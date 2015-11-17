yum.define([
	
], function () {

	Class('Administradora.Model').Extend(Mvc.Model.Base).Body({

		instances: function () {

		},

		init: function () {
			this.base.init('/Administradora');
		},

		validations: function () {
			return {
				'Nome': new Mvc.Model.Validator.Required('Informe o nome da administradora')
			};
		},

		initWithJson: function (json) {
			var model = new Administradora.Model(json);

			model.Endereco = Endereco.Model.create().initWithJson(json.Endereco);

			return model;
		},

		actions: {
			'get': '/get?Id=:Id'
		}

	});
});