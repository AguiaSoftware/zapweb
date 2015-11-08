yum.define([
	PI.Url.create('Public', '/ui.config.js'),
    PI.Url.create('Public', '/app.css'),
    PI.Url.create('Auth', '/page.js')
], function (html) {
    
    Class('App').Extend(Mvc.Component).Body({

        viewDidLoad: function () {
            var auth = new Auth.Page();

            auth.render(this.view.body);

            this.base.viewDidLoad();

        }

    });

});