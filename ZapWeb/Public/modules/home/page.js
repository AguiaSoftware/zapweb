yum.define([
	PI.Url.create('Home', '/page.html'),
	PI.Url.create('Home', '/page.css'),
    PI.Url.create('Usuario', '/label/label.js'),
    PI.Url.create('Unidade', '/label/label.js'),
    PI.Url.create('Notificacao', '/button/button.js')
], function (html) {

    Class('Home.Page').Extend(Mvc.Component).Body({

        instances: function () {
            this.view = new Mvc.View(html);

            this.menubar = new UI.Menu.MenuBar();
            this.usuario = new Usuario.Label();
            this.unidade = new Unidade.Label();
            this.notificacao = new Notificacao.Button();

            this.logotipo = PI.Url.create('Home', '/logotipo.png');

            this.currentPage = null;
        },

        viewDidLoad: function () {
            var cosId = Unidade.Current.Tipo == Unidade.Tipo.COS ? Unidade.Current.Id : 0;
            var centralId = Unidade.Current.Tipo == Unidade.Tipo.CENTRAL ? Unidade.Current.Id : 0;

            this.adicionar = new UI.Menu.Menu({ label: 'Adicionar' });
            this.pesquisar = new UI.Menu.Menu({ label: 'Pesquisar' });
            this.relatorio = new UI.Menu.Menu({ label: 'Relatório' });
            this.configuracao = new UI.Menu.Menu({ label: 'Configuração' });

            this.menubar.add(this.adicionar);
            this.menubar.add(this.pesquisar);
            this.menubar.add(this.relatorio);

            if (Usuario.Current.hasPermissao('ADD_PERMISSAO')) {
                this.menubar.add(this.configuracao);

                //configuração
                this.configuracao.add(new UI.Menu.Item({ label: 'Permissões', href: 'Permissao/Gerenciar', icon: 'fa fa-university text-black', labelRight: 'Ctrl-H-A' }));
            }
            
            //adicionar
            if (Usuario.Current.hasPermissao('ADD_UNIDADE')) {
                this.adicionar.add(new UI.Menu.Item({ label: 'Unidade', href: 'Unidade/Adicionar', icon: 'fa fa-university text-black', labelRight: 'Ctrl-H-A' }));    
            }
            
            this.adicionar.add(new UI.Menu.Item({ label: 'Fornecedor', href: 'Fornecedor/Adicionar', icon: 'fa fa-dropbox text-black', labelRight: 'Ctrl-H-A' }));
            this.adicionar.add(new UI.Menu.Separador());

            if (Usuario.Current.hasPermissao('ADD_USUARIO')) {
                this.adicionar.add(new UI.Menu.Item({ label: 'Usuário', href: 'Usuario/Adicionar', icon: 'glyphicon glyphicon-user text-black', labelRight: 'Ctrl-H-A' }));
            }

            this.adicionar.add(new UI.Menu.Separador());

            if (Usuario.Current.hasPermissao('ADD_RECEITA')) {
                this.adicionar.add(new UI.Menu.Item({ label: 'Receita', href: 'Receita/Adicionar', icon: 'fa fa-plus-circle text-black', labelRight: 'Ctrl-H-A' }));                
            }
            
            if (Usuario.Current.hasPermissao('ADD_DESPESA')) {
                this.adicionar.add(new UI.Menu.Item({ label: 'Despesa', href: 'Despesa/Adicionar', icon: 'fa fa-minus-circle text-black', labelRight: 'Ctrl-H-A' }));
            }            

            //pesquisar
            if (Usuario.Current.hasPermissao('UPDATE_UNIDADE')) {
                this.pesquisar.add(new UI.Menu.Item({ label: 'Unidade', href: 'Unidade/Pesquisar', icon: 'fa fa-university text-black', labelRight: 'Ctrl-H-A' }));
            }            

            this.pesquisar.add(new UI.Menu.Item({ label: 'Fornecedor', href: 'Fornecedor/Pesquisar', icon: 'fa fa-dropbox text-black', labelRight: 'Ctrl-H-A' }));
            this.pesquisar.add(new UI.Menu.Separador());

            if (Usuario.Current.hasPermissao('UPDATE_USUARIO')) {
                this.pesquisar.add(new UI.Menu.Item({ label: 'Usuário', href: 'Usuario/Pesquisar', icon: 'glyphicon glyphicon-user text-black', labelRight: 'Ctrl-H-A' }));
            }            

	        this.pesquisar.add(new UI.Menu.Separador());

            if (Usuario.Current.hasPermissao('VIEW_RECEITA')) {
                this.pesquisar.add(new UI.Menu.Item({ label: 'Receita', href: 'Receita/Pesquisar', icon: 'fa fa-plus-circle text-black', labelRight: 'Ctrl-H-A' }));
            }	        

            if (Usuario.Current.hasPermissao('UPDATE_DESPESA')) {
                this.pesquisar.add(new UI.Menu.Item({ label: 'Despesa', href: 'Despesa/Pesquisar', icon: 'fa fa-minus-circle text-black', labelRight: 'Ctrl-H-A' }));
            }	        

            //relatorio
            if (Usuario.Current.hasPermissao('RELATORIO_DESPESA_UNIDADE')) {
                this.relatorio.add(new UI.Menu.Item({ label: 'Despesa - Unidade', href: 'Relatorio/Despesa/Unidade/' + cosId, icon: 'fa fa-file-text-o text-black', labelRight: 'Ctrl-H-A' }));    
            }

            if (Usuario.Current.hasPermissao('RELATORIO_DESPESA_CENTRAL')) {
                this.relatorio.add(new UI.Menu.Item({ label: 'Despesa - Central', href: 'Relatorio/Despesa/Central/' + centralId, icon: 'fa fa-file-text-o text-black', labelRight: 'Ctrl-H-A' }));    
            }
            
            if (Usuario.Current.hasPermissao('RELATORIO_DESPESA_ZAP')) {
                this.relatorio.add(new UI.Menu.Item({ label: 'Despesa - Zap', href: 'Relatorio/Despesa/Zap', icon: 'fa fa-file-text-o text-black', labelRight: 'Ctrl-H-A' }));                
            }

            this.relatorio.add(new UI.Menu.Separador());

            if (Usuario.Current.hasPermissao('RELATORIO_RECEITA_UNIDADE')) {
                this.relatorio.add(new UI.Menu.Item({ label: 'Receita - Unidade', href: 'Relatorio/Receita/Unidade/0', icon: 'fa fa-file-text-o text-black', labelRight: 'Ctrl-H-A' }));            
            }

            if (Usuario.Current.hasPermissao('RELATORIO_RECEITA_CENTRAL')) {
                this.relatorio.add(new UI.Menu.Item({ label: 'Receita - Central', href: 'Relatorio/Receita/Central/0', icon: 'fa fa-file-text-o text-black', labelRight: 'Ctrl-H-A' }));            
            }

            if (Usuario.Current.hasPermissao('RELATORIO_RECEITA_ZAP')) {
                this.relatorio.add(new UI.Menu.Item({ label: 'Receita - Zap', href: 'Relatorio/Receita/Zap', icon: 'fa fa-file-text-o text-black', labelRight: 'Ctrl-H-A' }));
            }

            if (!PI.Navigator.isChrome()) {
                this.view.infoNav.show();
            }
            
            if (Usuario.Current.UnidadeId != Unidade.Current.Id) {
                this.view.infoUnidade.show();
            }

            this.base.viewDidLoad();
        },

        setTitle: function (title) {
            if (title.length == 0) this.view.title.html('');
            else this.view.title.html(' /&nbsp;&nbsp;' + title || '');
        },

        setPage: function (page) {

            if (this.currentPage != null) {
                this.currentPage.destroy();
                this.view.content.html('');
            }

            this.currentPage = page;

            page.render(this.view.content, {
                anime: true,
                append: false
            });
        },

        setModal: function (modal) {
            modal.render(this.view.content, {
                anime: true,
                append: true
            });
        },

        events: {

            '@home click': function () {
                window.location = PI.Url.create('BaseUrl', '/');
            }

        }

    });

});