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

                // DataGrid
                $.extend($.fn.datagrid.defaults, {
                    loader: function (param, success, error) {

                        var opts = $(this).datagrid('options');

                        if (!opts.url) return false;

                        $.ajax({
                            type: opts.method,
                            url: opts.url,
                            data: param,
                            dataType: 'json',
                            success: function (result) {

                                if (result.success == false) {
                                    this.showError(result);
                                    success({
                                        total: 0,
                                        rows: []
                                    });
                                }

                                success(result);
                            },
                            error: function (jqXHR, textStatus) {
                                this.showAjaxError(jqXHR, textStatus);
                                error.apply(this, arguments);
                            }
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
                return '<div class="workitem-link" style="display:inline-block;"><a href="https://dev.azure.com/mihvsts/UIS/_workitems/edit/' + id + '&fullScreen=true" target="_blank">' + id + '</a></div>';
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

            },

            renderIcon: function (iconCls, tooltip) {

                var result = $('<div></div>');

                var div = $('<div></div>', {
                    style: 'width:16px; height:16px; display:inline-block; vertical-align: middle;',
                    class: iconCls
                }).appendTo(result);

                return result.html();
            },

            renderState: function (state) {

                switch (state) {
                    case 'Active':
                        return this.renderIcon('ico-state-active') + '&nbsp' + state;
                    case 'Closed':
                        return this.renderIcon('ico-state-closed') + '&nbsp' + state;
                    case 'Proposed':
                        return this.renderIcon('ico-state-proposed') + '&nbsp' + state;
                    default:
                        return state;
                }
            },

            // Вывод ошибки AJAX запроса
            showAjaxError: function (jqXHR, textStatus) {

                var params = {
                    title: 'Ошибка',
                    icon: 'error'
                };

                if (jqXHR.status === 0) {
                    params.msg = 'Нет соединения с сервером';
                    params.icon = undefined;
                } else if (jqXHR.status == 404) {
                    params.msg = 'Запрашиваемый ресурс не найден. [404]';
                } else if (jqXHR.status == 500) {
                    params.msg = 'Внутренняя ошибка сервера [500]';
                } else {
                    params.msg = 'Произошла ошибка. ' + textStatus + ' [' + jqXHR.status + ']'
                }

                $.messager.alert(params);
            },

            // Вывод ошибки
            showError: function (result) {

                // Если просто ошибка
                if (result.error !== undefined) {
                    $.messager.alert({
                        title: 'Ошибка',
                        icon: 'error',
                        width: 400,
                        msg: result.error
                    });
                }
                // Если исключение
                else if (result.exception !== undefined) {
                    $.messager.alert({
                        title: 'Ошибка',
                        icon: 'error',
                        width: 400,
                        msg: 'Во время выполнения возникло исключение:</br>' + result.exception
                    });
                }
            }
        }
    });

})(jQuery);