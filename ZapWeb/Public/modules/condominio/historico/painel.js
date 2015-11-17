yum.define([
	PI.Url.create('Condominio', '/historico/painel.html'),
	// PI.Url.create('Condominio', '/historico/painel.css'),
	PI.Url.create('Condominio', '/historico/popup.js')
], function(html){

	Class('Condominio.Historico.Painel').Extend(Mvc.Component).Body({

		instances: function(){
			this.view = new Mvc.View(html);
			
			this.add = new UI.Button({
				iconLeft: 'text-success fa fa-plus',
				label: 'Adicionar contato',
				classes: 'cinza'
			});
			
			this.newrow = new Condominio.Historico.TableRowNew({
				dataModel: 'model'
			});
			
			// this.popup = new Condominio.Historico.Popup();
			this.model = new Condominio.Historico.Model();
		},
		
		viewDidLoad: function(){
			
			// this.popup.showOnClick( this.add );
			
			this.base.viewDidLoad();
		},
		
		load: function( condominio ){
			var self = this;
			
			this.model.Condominio = condominio;
			
			this.model.all( condominio.Id ).ok(function(historicos){
				self.popule(historicos);
			});
		},
		
		popule: function(historicos){
			
			this.view.tbody.html('');			
			
			for (var i = 0; i < historicos.length; i++) {
				var h = historicos[i];
				
				this.addRow(h);
			}
		},
		
		addRow: function(historico){
			var row = new Condominio.Historico.TableRow({
				historico: historico
			});

			row.render( this.view.tbody );
		},
		
		events: {
		
			'{add} click': function(){
				var self = this;
				var model = new Condominio.Historico.Model({
					Condominio: this.model.Condominio
				});

				this.saveModel( model, this.add ).ok(function(historico){
					self.addRow(historico);
					self.newrow.clear();
					
					self.event.trigger('added', historico);
				});
			},
			
		}
		

	});

});