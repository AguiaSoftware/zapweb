yum.define([
	PI.Url.create('Condominio', '/historico/popup.html'),
	PI.Url.create('Condominio', '/historico/popup.css')
], function(html){

	Class('Condominio.Historico.Popup').Extend(UI.Popup).Body({

		instances: function(){
			this.view.inject({
				content: html
			});
			
			this.rank = new UI.Rating();
			
			this.data = new UI.DateBox({
				placeholder: 'Data'
			});
			
			this.proximo = new UI.DateBox({
				placeholder: 'Data'
			});
			
			this.descricao = new UI.TextArea({
				placeholder: 'Descrição',
				autosize: true
			});
			
			this.__salvar = new UI.Button({
				classes: 'verde',
				label: 'Salvar'
			});
			
			this.position = 'left::top';
		},
		
		viewDidLoad: function(){
			this.view.header.hide();
			
			this.base.viewDidLoad();
		}

	});

});