yum.define([

], function () {

    Class('Arquivo.Model').Extend(Mvc.Model.Base).Body({

        instances: function () {

        },

        init: function () {
            this.base.init('/Arquivo');
        },

        onValid: function () {
            return {
                //'': new Mvc.Model.Validator.Required('')
            };
        },

        initWithJson: function (json) {
            var model = new Arquivo.Model(json);

            return model;
        },

        getImageUrl: function(){
            if(this.Tipo.indexOf('word') > 0){
                return PI.Url.create('Arquivo', '/word.png');
            }else if (this.Tipo == 'application/pdf') {
                return PI.Url.create('Arquivo', '/pdf.png').getUrl();
            }else{
                return PI.Url.create('BaseUrl', '/Arquivo/Thumb?hash=' + this.Hash).getUrl();
            }
        },

        getUrl: function(){
            if (this.Tipo == 'application/pdf') {
                return PI.Url.create('BaseUrl', '/Arquivo/Visualizar?hash=' + this.Hash).getUrl();
            }else{
                return PI.Url.create('BaseUrl', '/Arquivo/Thumb?hash=' + this.Hash).getUrl();
            }
            return ;
        },

        actions: {
            
        }

    });

});