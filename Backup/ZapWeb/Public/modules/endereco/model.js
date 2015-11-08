yum.define([
    PI.Url.create('Cidade', '/model.js')
], function () {

    Class('Endereco.Model').Extend(Mvc.Model.Base).Body({

        instances: function () {

        },

        init: function () {
            this.base.init('/Endereco');
        },

        onValid: function () {
            return {
                //'': new Mvc.Model.Validator.Required('')
            };
        },

        loadModelFromJson: function (json) {
            var model = new Endereco.Model(json);

            if (model.Cidade != null) {
                model.Cidade = Cidade.Model.create().loadModelFromJson(json.Cidade);
            }

            return model;
        },

        actions: {

        }

    });

});