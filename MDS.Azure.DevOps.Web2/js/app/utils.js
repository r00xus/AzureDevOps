(function ($) {

    $.extend({

        utils: {

            // Инициализация
            init: function () {

                moment.locale("ru");

                numeral.locale('ru');
                numeral.nullFormat('');

                this.initEasyUi();
                this.initCookie();
            },

            initCookie: function () {

                $.cookie.json = true;

                $.cookie.defaults.expires = 365;
            },

            initEasyUi: function () {

                // DateBox
                $.extend($.fn.datebox.defaults, {
                    formatter: function (date) {
                        return moment(date).format('L');
                    },
                    parser: function (s) {
                        if (!s) {
                            return new Date();
                        }
                        var parsedMoment = moment(s, ["L", moment.ISO_8601]);
                        if (parsedMoment.isValid()) {
                            return parsedMoment.toDate();
                        } else {
                            return new Date();
                        }
                    }
                });

                // Numberbox
                $.extend($.fn.numberbox.defaults, {
                    decimalSeparator: ',',
                    groupSeparator: ' '
                });

                // Window
                $.extend($.fn.window.defaults, {
                    collapsible: false,
                    minimizable: false,
                    maximizable: false,
                });

                // Panel
                $.extend($.fn.panel.methods, {
                    // Вывод маски загрузки с сообщением
                    loading: function (jq, msg) {
                        return jq.each(function () {

                            var panelBody = $(this).panel('body');

                            panelBody.css('position', 'relative');

                            var maskDiv = $('<div></div>', {
                                class: 'datagrid-mask',
                                style: 'display:block'
                            }).appendTo(panelBody);

                            var messageDiv = $('<div></div>', {
                                class: 'datagrid-mask-msg',
                                style: 'display: block; left: 50%; height: 40px; margin-left: -89.2031px; line-height: 16px;'
                            }).appendTo(panelBody);

                            messageDiv.html(msg);
                        });
                    },
                    // Удаление маски
                    loaded: function (jq) {
                        return jq.each(function () {

                            var panelBody = $(this).panel('body');

                            $(panelBody).children('div.datagrid-mask ').remove();
                            $(panelBody).children('div.datagrid-mask-msg').remove();

                        });
                    }
                });

            },

            // Форматирование часов
            formatHours: function (val) {

                var num = parseFloat(val);

                if (num == NaN) return null;

                return numeral(num).format('0,0.00');
            },

            // Форматирование даты
            formatDate: function (val) {
                if (val == null || val == '') return null;
                var parse = moment(val, ["L", moment.ISO_8601]);
                return parse.format('L');
            },

            renderWorkItemLink: function (id) {
                if (id == null || id === undefined) return null;
                return '<div class="workitem-link"><a href="https://dev.azure.com/mihvsts/UIS/_workitems/edit/' + id + '&fullScreen=true" target="_blank">' + id + '</a></div>';
            },

            setWorkItemLinks: function (target) {

                target.find('.workitem-link a').each(function () {

                    var link = $(this);

                    $(this).click(function (e) {

                        e.preventDefault();

                        var width = 1200;
                        var height = 800;
                        var left = (window.screen.width / 2) - ((width / 2) + 10);
                        var top = (window.screen.height / 2) - ((height / 2) + 50);

                        window.open($(this).attr('href'), 'xml', 'left=' + left + ',top=' + top + ',width=' + width + ',height=' + height + ',resizable,scrollbars');

                    });
                });

            }
        }
    });

})(jQuery);