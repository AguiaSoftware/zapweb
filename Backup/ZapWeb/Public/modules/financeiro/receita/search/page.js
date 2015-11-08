yum.define([
	PI.Url.create('Financeiro', '/receita/search/page.html'),
	PI.Url.create('Financeiro', '/receita/search/page.css'),
	PI.Url.create('Financeiro', '/receita/search/row.js')
], function(html){

	Class('Financeiro.Receita.Search.Page').Extend(Mvc.Component).Body({

		instances: function(){
			this.view = new Mvc.View(html);

			this.model = new Financeiro.Receita.Model();
		},

		viewDidLoad: function(){
			var self = this;

			app.home.setTitle('Pesquisar Receita');

			this.model.all(Unidade.Current.Id).ok(function(receitas){
				self.fill(receitas);
			});

		    this.base.viewDidLoad();
		},

		fill: function(receitas){
			this.view.tbody.html('');

			for(var i = 0 ; i < receitas.length ; i++){
			   var row = new Financeiro.Receita.Search.TableRow({
			   		number: i,
					receita: receitas[i]
			   });

				row.render(this.view.tbody);
			}
		 }

	});

});