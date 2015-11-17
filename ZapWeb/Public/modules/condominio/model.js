yum.define([
	
], function () {

	Class('Condominio.Model').Extend(Mvc.Model.Base).Body({

		instances: function () {

		},

		init: function () {
			this.base.init('/Condominio');
		},

		validations: function () {
			return {
				'Nome': new Mvc.Model.Validator.Required('Informe o nome do condomínio')
			};
		},

		initWithJson: function (json) {
			var model = new Condominio.Model(json);

			model.Unidade = Unidade.Model.create().initWithJson(model.Unidade);
			model.DataUltimaCampanha = Lib.DataTime.create(json.DataUltimaCampanha, 'yyyy-MM-ddThh:mm:ss').getDateStringFromFormat('dd/MM/yyyy');
			model.Endereco = Endereco.Model.create().initWithJson(json.Endereco);
			
			return model;
		},

		actions: {
			
		}

	});
});