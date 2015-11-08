yum.define([

], function () {

    Class('Financeiro.Receita.Model').Extend(Mvc.Model.Base).Body({

        instances: function () {

        },

        init: function () {
            this.base.init('/Receita');
        },

        onValid: function () {
            return {
                'Mes': new Mvc.Model.Validator.Required('Mês é obrigatório'),
                'Ano': new Mvc.Model.Validator.Required('Ano é obrigatório'),
                'Unidade': new Mvc.Model.Validator.Required('Unidade é obrigatório')
            };
        },

        loadModelFromJson: function (json) {
            var model = new Financeiro.Receita.Model(json);

            if (json == null) return model;

	        model.Mes = Lib.DataTime.ConvertIndexToMes(json.Mes);

            return model;
        },

        actions: {
            'all': '/all?unidadeId=:unidadeId',
	        'find': '/find?mes=:mes&ano=:ano&unidadeId=:unidadeId'
        }

    });

});