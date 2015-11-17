yum.define([
	PI.Url.create('Lib', '/rating/raty.js'),
	PI.Url.create('Lib', '/rating/raty.css')
], function(html){

	Class('UI.Rating').Extend(Mvc.Component).Body({

		instances: function(){
			this.view = new Mvc.View('<div></div>');
			
			this.readOnly = false;
			this.cancel = false;
		},
		
		viewDidLoad: function(){
		
			$.fn.raty.defaults.path = PI.Url.create('Lib', '/rating/images').getUrl();
		
			this.view.element.raty({
				cancel: this.cancel,
				readOnly: this.readOnly
			});
			
			this.base.viewDidLoad();
		},
		
		get: function(){
			return this.view.element.raty('score');
		},
		
		set: function(rank){
			this.view.element.raty({
				score: rank,
				readOnly: this.readOnly
			});
		},
		
		reload: function(){
			this.view.element.raty('reload');
		}

	});

});