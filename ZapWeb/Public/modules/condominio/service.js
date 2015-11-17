yum.define([
	PI.Url.create('Condominio', '/page/page.js'),
	PI.Url.create('Condominio', '/search/page.js'),
	PI.Url.create('Condominio', '/material/page/page.js'),
	PI.Url.create('Condominio', '/material/model.js'),
	PI.Url.create('Condominio', '/model.js')
], function (html) {

	Class('Service.Condominio').Extend(PI.Service).Body({

		load: function(app){
			this.base.load(app);
		},

		routes: {
			
			'Condominio/Adicionar': function(){
				setTimeout(function() {
					var page = new Condominio.Page({
						model: new Condominio.Model()
					});
					
					app.home.setPage( page );
				}, 1);
			},			
			
			'Condominio/Editar/:Id': function(Id){
				setTimeout(function() {
					var page = new Condominio.Page({
						model: new Condominio.Model({
							Id: Id
						})
					});
					
					app.home.setPage( page );
				}, 1);
			},			
			
			'Condominio/Pesquisar': function(){
				setTimeout(function() {
					var page = new Condominio.Search.Page();
					
					app.home.setPage( page );
				}, 1);
			},
			
			'Condominio/Gerar/Material/:Id': function(condominioId){
				setTimeout(function() {
					var page = new Condominio.Material.Page({
						model: new Condominio.Material.Model({
							Condominio: new Condominio.Model({
								Id: condominioId
							})
						})
					});
					
					app.home.setPage( page );
				}, 1);
			},
			
			'Condominio/Editar/Material/:Id': function(materialId){
				setTimeout(function() {
					var page = new Condominio.Material.Page({
						model: new Condominio.Material.Model({
							Id: materialId
						})
					});
					
					app.home.setPage( page );
				}, 1);
			}
			
		},

		events: {
			
		}

	});

});