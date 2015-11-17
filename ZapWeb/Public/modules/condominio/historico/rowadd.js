yum.define([
	// PI.Url.create('Condominio', '/historico/rowadd.html'),
	// PI.Url.create('Condominio', '/historico/rowadd.css')
], function(html){

	Class('Condominio.Historico.TableRowNew').Extend(Mvc.Component).Body({

		instances: function(){
			this.view = new Mvc.View('<tr> <td at="data" style="text-align: left;"></td> <td at="descricao"></td> <td at="proximo" style="text-align: left;"></td> <td at="rank" style="text-align: left"></td> </tr>');

			this.data = new UI.DateBox({
				dataModel: 'Data',
				placeholder: 'Data'
			});
			
			this.proximo = new UI.DateBox({
				dataModel: 'ProximoContato',
				placeholder: 'Data'
			});
			
			this.descricao = new UI.TextBox({
				dataModel: 'Descricao',
				placeholder: 'Descrição'
			});
			
			this.rank = new UI.Rating({
				dataModel: 'Rank'
			});
		},
		
		get: function(model){
			var s = this.injectViewToModel(model);
			var data = this.data.get();
			var proximo = this.proximo.get();
			
			if(data.length > 0 && proximo.length > 0){
				var d1 = new Lib.DataTime(data, 'dd/MM/yyyy');
				var d2 = new Lib.DataTime(proximo, 'dd/MM/yyyy');
				
				if(d1.compareTo(d2) < 0){
					throw 'A data do próximo contato deve ser maior que a data informada';
				}
			}
						
			if(!s.status){
				throw s.messages;
			}
		},
		
		clear: function(){
			this.data.set('');
			this.proximo.set('');
			this.descricao.set('');
			this.rank.set(0);
		}

	});

});