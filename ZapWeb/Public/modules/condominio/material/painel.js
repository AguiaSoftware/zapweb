yum.define([
	PI.Url.create('Condominio', '/material/painel.html'),
	PI.Url.create('Condominio', '/material/painel.css'),
	PI.Url.create('Condominio', '/material/item.js'),
	PI.Url.create('Condominio', '/material/modal/modal.js'),
], function(html){

	Class('Condominio.Material.Painel').Extend(Mvc.Component).Body({

		instances: function(){
			this.view = new Mvc.View(html);
			
			this.gerar = new UI.Button({
				iconLeft: 'fa fa-plus',
				label: 'Gerar'
			});
			
			this.model = new Condominio.Material.Model();
		},
		
		load: function(condominio){
			this.model.Condominio = condominio;
			
			this.refresh();
		},
		
		refresh: function(){
			var self = this;
			
			this.model.all( this.model.Condominio.Id ).ok(function(materiais){
				self.popule(materiais);
				console.log(materiais);
			});
		},
		
		popule: function(materiais){
			
			this.view.results.html('');
			
			for (var i = 0; i < materiais.length; i++) {
				var m = materiais[i];
				this.add(m);
			}
		},
		
		add: function(material){
			var item = new Condominio.Material.Item({
				material: material
			});
			
			item.render( this.view.results );
		},
		
		events: {
		
			'{gerar} click': function(){
				PI.Url.Hash.to('Condominio/Gerar/Material/' + this.model.Condominio.Id);
			},
			
			'{EventGlobal} added::material': function(material){
				this.refresh();
			}
		
			// '{gerar} click': function(){
			// 	var modal = new Condominio.Material.Modal();
				
			// 	modal.render( this.view.element );
				
			// 	modal.open();
			// }
		}

	});

});