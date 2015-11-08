yum.define([
    
], function () {

    Class('GrupoPermissao.Model').Extend(Mvc.Model.Base).Body({

        instances: function () {
            
        },

        init: function () {
            this.base.init('/GrupoPermissao');
        },

        onValid: function () {
            return {
                'Nome': new Mvc.Model.Validator.Required('O nome do grupo é obrigatório')
            };
        },

        actions: {
            'addPermissoes': '/addPermissoes'
        }

    });

});