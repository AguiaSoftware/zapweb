yum.define([
	
], function (html) {

    Class('UI.RadioBox').Extend(Mvc.Component).Body({

        instances: function () {
            this.view = new Mvc.View('<div class="radio"><label class="i-checks"><input at="radio" type="radio" name="@{group}" value="@{value}"><i class="@{classes}"></i></label></div>');
        },

        set: function (value) {
            if (this.value == value) {
                this.view.radio.prop('checked', true);
            }
        },

        get: function () {
            return this.view.radio.is(':checked') ? this.value : Mvc.Model.Type.IGNORE;
        }

    });

});