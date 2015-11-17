yum.define([
	
], function () {

	Class('Agenda.Model').Extend(Mvc.Model.Base).Body({

		instances: function () {

		},

		init: function () {
			this.base.init('/Agenda');
		},

		validations: function () {
			return {
				//'': new Mvc.Model.Validator.Required('')
			};
		},

		initWithJson: function (json) {
			var model = new Agenda.Model(json);
			
			model.Data = Lib.DataTime.create(json.Data, 'yyyy-MM-ddThh:mm:ss').getDateStringFromFormat('yyyy-MM-dd hh:mm:ss');
			
			return model;
		},

		actions: {
			'feed': '/feed?start=:start&end=:end'
		}

	});
});