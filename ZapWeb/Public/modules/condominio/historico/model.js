yum.define([
	
], function () {

	Class('Condominio.Historico.Model').Extend(Mvc.Model.Base).Body({

		instances: function () {

		},

		init: function () {
			this.base.init('/Historico');
		},

		validations: function () {
			return {
				'Data': new Mvc.Model.Validator.Required('Informe a data do contato'),
				'Descricao': new Mvc.Model.Validator.Required('Informe a descrição do contato'),
				'ProximoContato': new Mvc.Model.Validator.Required('Informe a data do próximo contato'),
				'Rank': new Mvc.Model.Validator.Required('Informe a classificação do condomínio')
			};
		},

		initWithJson: function (json) {
			var model = new Historico.Model(json);

			model.Data = Lib.DataTime.create(json.Data, 'yyyy-MM-ddThh:mm:ss').getDateStringFromFormat('dd/MM/yyyy');
			model.ProximoContato = Lib.DataTime.create(json.ProximoContato, 'yyyy-MM-ddThh:mm:ss').getDateStringFromFormat('dd/MM/yyyy');

			return model;
		},

		actions: {
			'all': '/all?condominioId=:CondominioId'
		}

	});
});