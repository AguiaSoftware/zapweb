yum.define([
	PI.Url.create('Usuario', '/table/row.html'),
	PI.Url.create('Usuario', '/table/row.css')
], function (html) {

    Class('Usuario.TableRow').Extend(Mvc.Component).Body({

        instances: function () {
            this.view = new Mvc.View(html);

            this.excluir = new UI.Button({
                label: 'Excluir',
                classes: 'vermelho'
            });

            this.editar = new UI.Button({
                label: 'Editar',
                classes: 'cinza'
            });

            this.columns = ['excluir', 'editar'];
        },

        viewDidLoad: function () {
            if (this.isOdd) {
                this.view.element.addClass('flat-table-row-odd');
            }

            this.view.element.evidence();

            this.showColumns();

            this.base.viewDidLoad();
        },

        showColumns: function () {
            for (var i = 0; i < this.columns.length; i++) {
                this.view[this.columns[i]].show();
            }
        },

        events: {

            '{editar} click': function () {
                PI.Url.Hash.to('Usuario/Editar/' + this.usuario.Id);
            },

            '{excluir} click': function () {
                this.event.trigger('destroy');
                this.destroy();                
            }

        }

    });

});