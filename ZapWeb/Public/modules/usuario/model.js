yum.define([
    PI.Url.create('Auth', '/model.js'),
    PI.Url.create('Unidade', '/model.js')
], function () {

    Class('Usuario.Model').Extend(Mvc.Model.Base).Body({

        instances: function () {
            this.Account = new Auth.Model();
        },

        init: function () {
            this.base.init('/Usuario');

            this.Unidade = new Unidade.Model(this.Unidade);
        },

        onValid: function () {
            return {
                'Nome': new Mvc.Model.Validator.Required('O nome do usuário é obrigatório'),
                'Email': new Mvc.Model.Validator.Required('O email do usuário é obrigatório')
            };
        },

        initWithJson: function (json) {
            var model = new Usuario.Model(json);

            model.Account = Auth.Model.create().initWithJson(json.Account);

            return model;
        },

        actions: {
            'all': '/all?unidadeId=:unidadeId'
        },

        hasPermissao: function(permissao){
            
            for (var i = this.Permissoes.Permissoes.length - 1; i >= 0; i--) {
                if (this.Permissoes.Permissoes[i].Nome == permissao) return true;
            }

            return false;
        }

    });

    Usuario.Current = new Usuario.Model(CurrentUsuario);
    
    if (PI.Cookie.get('unidade') == null) {
        Unidade.Model.persistir( Usuario.Current.Unidade );
        Unidade.Current = Usuario.Current.Unidade;
    }else{
        Unidade.Current = new Unidade.Model( JSON.parse( PI.Cookie.get('unidade') ) );
    }

});