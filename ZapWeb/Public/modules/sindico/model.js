yum.define([
	
], function () {

	Class('Sindico.Model').Extend(Mvc.Model.Base).Body({

		instances: function () {

		},

		init: function () {
			this.base.init('/Sindico');
		},

		validations: function () {
			return {
				'Nome': new Mvc.Model.Validator.Required('Informe o nome do síndico')
			};
		},

		initWithJson: function (json) {
			var model = new Sindico.Model(json);

			return model;
		},

		actions: {
			
		}

	});
});