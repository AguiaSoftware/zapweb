yum.define([
	// PI.Url.create('Condominio', '/historico/row.html'),
	// PI.Url.create('Condominio', '/historico/row.css')
], function(html){

	Class('Condominio.Historico.TableRow').Extend(Mvc.Component).Body({

		instances: function(){
			this.view = new Mvc.View('<tr> <td at="data">@{this.historico.Data}</td> <td at="descricao">@{this.historico.Descricao}</td> <td at="proxima" style="text-align: center;">@{this.historico.ProximoContato}</td> <td at="rank" style="text-align: center"></td> </tr>');
			this.rank = new UI.Rating({
				readOnly: true
			});
		},
		
		viewDidLoad: function(){
			
			this.rank.set( this.historico.Rank );
			
			this.base.viewDidLoad();
		}

	});

});