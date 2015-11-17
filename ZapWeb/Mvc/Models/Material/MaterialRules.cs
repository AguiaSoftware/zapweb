using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Globalization;

using Microsoft.Office.Interop.Word;
using Ionic.Zip;

namespace ZapWeb.Models
{
    public class MaterialRules : ZapWeb.Lib.Mvc.BusinessLogic
    {

        public bool Adicionar(Material material)
        {
            var materialRepositorio = new MaterialRepositorio();

            if (!Account.Current.Permissao.Has("ADD_MATERIAL"))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }

            material.Usuario = Account.Current.Usuario;
            materialRepositorio.Insert(material);

            return true;
        }

        public bool Update(Material material)
        {
            var materialRepositorio = new MaterialRepositorio();

            if (!Account.Current.Permissao.Has("UPDATE_MATERIAL"))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }

            var current = materialRepositorio.Fetch(material.Id);
            material.CondominioId = current.CondominioId;
            material.UsuarioId = current.UsuarioId;


            material.Usuario = Account.Current.Usuario;
            materialRepositorio.Update(material);

            return true;
        }

        public Material Get(int Id)
        {
            var materialRepositorio = new MaterialRepositorio();
            var arquivoRules = new ArquivoRules();

            if (!Account.Current.Permissao.Has("UPDATE_MATERIAL"))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return null;
            }

            return materialRepositorio.Fetch(Id); 
        }

        public List<Material> All(int condominioId)
        {
            var materialRepositorio = new MaterialRepositorio();

            return materialRepositorio.FetchByCondominioId(condominioId);
        }

        public string GetPdfFilename(int materialId, string hash)
        {
            var materialRepositorio = new MaterialRepositorio();
            var unidadeRepositorio = new UnidadeRepositorio();
            var condominioRepositorio = new CondominioRepositorio();
            var usuarioRepositorio = new UsuarioRepositorio();
            var arquivoRules = new ArquivoRules();

            var arquivo = arquivoRules.GetByHash(hash);
            var filename = Pillar.Mvc.Application.Path("/Public/files/" + arquivo.Hash);
            var filenameCopy = this.GeneratorFileName(".docx");
            var filenamePdf = this.GeneratorFileName(".pdf");

            System.IO.File.Copy(filename, filenameCopy, true);

            var material = materialRepositorio.Simple(materialId);
            var condominio = condominioRepositorio.Simple(material.CondominioId);
            var unidade = unidadeRepositorio.Fetch(condominio.UnidadeId);
            var usuarios = usuarioRepositorio.FetchUsuariosByUnidade(unidade, false);

            var content = this.GetContentDocument(filename);
            var keywords = new Dictionary<string, string>();
            keywords.Add("_DATA_INICIO_", ConvertDateToString( material.DataInicio));
            keywords.Add("_DIA_INICIO_", material.DataInicio.Day.ToString());
            keywords.Add("_MES_INICIO_", ConvertDateToNomeMes(material.DataInicio));

            keywords.Add("_DIA_FIM_", material.DataFim.Day.ToString());
            keywords.Add("_MES_FIM_", ConvertDateToNomeMes(material.DataFim));
            keywords.Add("_DATA_FIM_", ConvertDateToString(material.DataFim));

            keywords.Add("_HORA_INICIO_", material.HoraInicio);
            keywords.Add("_HORA_FIM_", material.HoraFim);
            keywords.Add("_VALOR_A_VISTA_", ConvertValorToString( material.ValorAVista));
            keywords.Add("_VALOR_CHEQUE_", ConvertValorToString(material.ValorCheque));
            keywords.Add("_ACRESCIMO_", ConvertValorToString(material.Acrescimo));
            keywords.Add("_DESCONTO_", ConvertValorToString(material.Desconto));

            keywords.Add("_NOME_CONDOMINIO_", condominio.Nome);
            keywords.Add("_QUANTIDADE_ANDARES_POR_BLOCO_", condominio.QuantidadeAndaresBloco.ToString());
            keywords.Add("_QUANTIDADE_APTO_", condominio.QuantidadeApto.ToString());
            keywords.Add("_QUANTIDADE_BLOCOS_", condominio.QuantidadeBlocos.ToString());

            if (usuarios != null && usuarios.Count > 0)
            {
                keywords.Add("_TRATAMENTO_", usuarios[0].Tratamento);
                keywords.Add("_USUARIO_NOME", usuarios[0].Nome);
            }
            
            var newContent = Pillar.Util.Template.Inject(content, keywords);

            this.CreatePdf(filenameCopy, filenamePdf, newContent);

            return filenamePdf;
        }

        private string ConvertDateToNomeMes(DateTime date)
        {
            string[] meses = { "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro" };

            return meses[date.Month];
        }
        
        private string ConvertValorToString(decimal valor)
        {
            return string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", valor);
        }

        private string ConvertDateToString(DateTime date)
        {
            return date.Day + "/" + date.Month + "/" + date.Year;
        }

        private void CreatePdf(string filenameDoc, string filenamePdf, string content)
        {
            ZipFile zip2 = new ZipFile(filenameDoc);
            zip2.RemoveEntry("word/document.xml");
            zip2.AddEntry("word/document.xml", content, Encoding.UTF8);
            zip2.Save();

            Microsoft.Office.Interop.Word.Document wordDocument;
            Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
            wordDocument = appWord.Documents.Open(filenameDoc);
            wordDocument.ExportAsFixedFormat(filenamePdf, WdExportFormat.wdExportFormatPDF);
        }
        
        private string GetContentDocument(string filename)
        {            
            var content = "";
            
            using (ZipFile zip = ZipFile.Read(filename))
            {
                MemoryStream ms = new MemoryStream();
                zip["word/document.xml"].Extract(ms);
                ms.Seek(0, SeekOrigin.Begin);
                var sr = new StreamReader(ms);
                content = sr.ReadToEnd();
            }

            return content;
        }

        private string GeneratorFileName(string extension)
        {
            return Pillar.Mvc.Application.Path("/Public/files/" + Guid.NewGuid().ToString() + extension);
        }

    }
}