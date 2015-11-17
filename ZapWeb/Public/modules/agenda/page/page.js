yum.define([
	PI.Url.create('Agenda', '/page/page.html'),
	PI.Url.create('Agenda', '/page/page.css'),
	PI.Url.create('Agenda', '/model.js')
], function(html){

	Class('Agenda.Page').Extend(PI.Page).Body({

		instances: function(){
			this.view = new Mvc.View(html);
			
			this.title = 'Agenda';
			
			this.voltar = new UI.Button();
			
			this.model = new Agenda.Model();
		},
		
		init: function(){
			this.base.init();
			
			this.calendar = new UI.Calendar({
				viewMode: this.viewMode
			});
		},
		
		viewDidLoad: function(){
			this.base.viewDidLoad();
			
			this.calendar.refresh();
		},
		
		events: {
		
			'{calendar} refresh': function(start, end, cb){
				
				this.model.feed(start.format(), end.format()).ok(function(items){
					var arr = [];
					
					for (var i = 0; i < items.length; i++) {
						arr.push({
							id: items[i].Id,
							title: items[i].Descricao,
							description: items[i].Descricao,
							start: items[i].Data,
							end: items[i].DataFinal,
							url: items[i].Url
						});
					}
					
					cb(arr);
				});
			},
			
			'{calendar} update::datetime': function(obj){
				var agenda = new Agenda.Model({
					Id: obj.id,
					Data: obj.start.format('DD/MM/YYYY hh:mm:ss')
				});
				
				agenda.update();				
			}
		}

	});

});