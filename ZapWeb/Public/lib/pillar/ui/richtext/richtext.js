yum.define([
	PI.Url.create('UI.RichText', '/richtext.html'),
	PI.Url.create('UI.RichText', '/richtext.css'),
	PI.Url.create('UI.RichText', '/ckeditor.js'),
	PI.Url.create('UI.RichText', '/pt-br.js'),
	PI.Url.create('UI.RichText', '/styles.js'),
	PI.Url.create('UI.RichText', '/config.js'),
	PI.Url.create('UI.RichText', '/skins/office2013/editor.css'),
	PI.Url.create('UI.RichText', '/jquery.js')
], function(html){
	
	/**
	 * @class Ui.RichText
	 */
	Class('UI.RichText').Extend(Mvc.Component).Body({
	
		instances: function(){
			this.view = new Mvc.View(html);

			this.id = PI.Util.UUID();
		},

		viewDidLoad: function(){
			var self = this;
		    
			this.view.editor.ckeditor();
		    this.base.viewDidLoad();
		},

		/**
		 * Seta o conteudo o rich text
		 * 
		 * @method set
		 * @param {string} text
		 * @return {this}
		 */
		set: function(text){
		    CKEDITOR.instances[ this.id ].setData( text );

		    return this;
		},

		/**
		 * Retorna o conteudo em HTML do rich text
		 * 
		 * @method get
		 * @return {string}
		 */
		get: function(){
		   	return CKEDITOR.instances[ this.id ].getData();
		}
	
	});
	
});