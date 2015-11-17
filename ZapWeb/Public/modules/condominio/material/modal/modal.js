yum.define([
	PI.Url.create('Condominio', '/material/modal/modal.html'),
	// PI.Url.create('Condominio', '/material/modal/modal.css')
], function(html){

	Class('Condominio.Material.Modal').Extend(UI.Modal).Body({

		instances: function(){
			this.view.inject({
				title: 'Gerar Material',
				body: html
			});
			
			this.dataInicio = new UI.DateBox({
				placeholder: 'Data'
			});
			
			this.dataFim = new UI.DateBox({
				placeholder: 'Data'
			});
			
			this.horaInicio = new UI.TextBox({
				mask: 'hora',
				placeholder: 'Horário'
			});
			
			this.horaFim = new UI.TextBox({
				mask: 'hora',
				placeholder: 'Horário'
			});
			
			this.valorAVista = new UI.TextBox({
				mask: 'financeira',
				placeholder: 'R$ 0,00'
			});
			
			this.valorCheque = new UI.TextBox({
				mask: 'financeira',
				placeholder: 'R$ 0,00'
			});
			
			this.valorDuplex = new UI.TextBox({
				mask: 'financeira',
				placeholder: 'R$ 0,00'
			});
			
			this.desconto = new UI.TextBox({
				mask: 'financeira',
				placeholder: 'R$ 0,00'
			});
			
			this.upload = new UI.Upload({
				config: {
					extensions: ['docx'],
					maxSize: 20000,
					unidade: 'docx'
				}
			});
		}

	});

});