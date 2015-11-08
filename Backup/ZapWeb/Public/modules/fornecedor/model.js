yum.define([
    PI.Url.create('Endereco', '/model.js')
], function () {

    Class('Fornecedor.Model').Extend(Mvc.Model.Base).Body({

        instances: function () {
            
        },

        init: function () {
            this.base.init('/Fornecedor');
        },

        onValid: function () {
            return {
                'RazaoSocial': new Mvc.Model.Validator.Required('A Razão Social é obrigatório')
            };
        },

        loadModelFromJson: function (json) {
            var model = new Fornecedor.Model(json);

            model.Endereco = Endereco.Model.create().loadModelFromJson(json.Endereco);
            model.Contato = Contato.Model.create().loadModelFromJson(json.Contato);

            return model;
        },

        actions: {
            'get': '/get?Id=:Id'
        }

    });

});