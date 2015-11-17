yum.define([
	PI.Url.create('Condominio', '/material/item.html'),
	// PI.Url.create('Condominio', '/material/item.css')
], function(html){

	Class('Condominio.Material.Item').Extend(Mvc.Component).Body({

		instances: function(){
			this.view = new Mvc.View(html);			
		},
		
		viewDidLoad: function(){
			var tpl = '<a class="condominio-material-item-arquivo" id="@{Id}" href="javascript:void(0)">@{PI.File.filename(this.Nome)}</a>';
			var view = '';
			
			for (var i = 0; i < this.material.Anexos.length; i++) {
				view +=  Mvc.Helpers.tpl(this.material.Anexos[i], tpl);				
			}
			
			this.view.materialAnexos.html( view );
			
			this.base.viewDidLoad();
		},
		
		getAnexoById: function(id){
			for (var i = 0; i < this.material.Anexos.length; i++) {
				var anexo = this.material.Anexos[i];
				
				if(anexo.Id == id) return anexo;				
			}
		},
		
		events: {
		
			'.condominio-material-item-arquivo click': function(e){
				var el = $(e);
				var id = el.attr('id'); 
				var anexo = this.getAnexoById(id);
				
				anexo.Material = this.material;
				
				window.location = Condominio.Material.Model.create( anexo ).getUrlPdf();
			}
		}

	});

});