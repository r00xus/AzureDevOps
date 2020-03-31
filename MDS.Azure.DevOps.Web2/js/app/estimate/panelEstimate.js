(function ($) {

    $.widget("custom.panelEstimate", {

        _create: function () {

            var that = this;

            that.dtgridEstimate = $('#dtgridEstimate', that.element);
            that.dtgridEstimate.datagrid({
                toolbar: $('#tbarEstimate', that.element),
                url: '/Estimate/EstimateReport/',
                striped: true,
                singleSelect: true,
                checkOnSelect: false,
                selectOnCheck: false,
                rownumbers: true,
                remoteSort: false,
                nowrap: false,
                clickToEdit: false,
                dblclickToEdit: true,
                columns: [[
                    {
                        field: 'checkbox', checkbox: true,
                    },
                    {
                        field: 'specName', title: 'Название постановки', sortable: true, width: 490,
                        editor: {
                            type: 'textbox'
                        }
                    },
                    {
                        field: 'date', title: 'Дата оценки', sortable: true,
                        formatter: function (val) {
                            return $.utils.formatDate(val);
                        },
                        editor: {
                            type: 'datebox'
                        }
                    },
                    {
                        field: 'developer', title: 'Программист', sortable: true, width: 185,
                        editor: {
                            type: 'combobox',
                            options: {
                                url: '/Settings/Employees/'

                            }
                        }
                    },
                    {
                        field: 'analytic', title: 'Постановщик', sortable: true, width: 240,
                        editor: {
                            type: 'textbox'
                        }
                    },
                    {
                        field: 'estimateDeveloperHours', title: 'Оценка программиста', align: 'right', sortable: true,
                        formatter: function (val, row) {
                            return $.utils.formatHours(val);
                        },
                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 2
                            }
                        }
                    },
                    {
                        field: 'estimateReviewerHours', title: 'Оценка ответст.', align: 'right', sortable: true,
                        formatter: function (val, row) {
                            return $.utils.formatHours(val);

                        },
                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 2
                            }
                        }
                    },
                    {
                        field: 'estimateFactHours', title: 'Фактически', align: 'right', sortable: true,
                        formatter: function (val, row) {
                            return $.utils.formatHours(val);
                        },
                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 2
                            }
                        }
                    },
                    {
                        field: 'taskId', title: 'ID Task', sortable: true, align: 'center',
                        formatter: function (val) {
                            return $.utils.renderWorkItemLink(val);
                        },
                        editor: {
                            type: 'numberbox',
                            options: {
                                groupSeparator: ''
                            }
                        }
                    },
                    {
                        field: 'taskName', title: 'Название Task', sortable: true, width: 370,
                    },
                    {
                        field: 'state', title: 'State', sortable: true
                    },
                    {
                        field: 'start', title: 'Start', sortable: true,
                        formatter: function (val) {
                            return $.utils.formatDate(val);
                        }
                    },
                    {
                        field: 'end', title: 'End', sortable: true,
                        formatter: function (val) {
                            return $.utils.formatDate(val);
                        }
                    },
                    {
                        field: 'reviewer', title: 'Ответственный за проект', sortable: true,
                        editor: {
                            type: 'textbox'
                        }
                    },
                ]],
                onLoadSuccess: function () {
                    $.utils.setWorkItemLinks($(this).datagrid('getPanel'));
                }
            });

            that.dtgridEstimate.datagrid('enableCellEditing');

            // Кнопка Обновить
            that.btnRefresh = $('#btnRefresh', that.element);
            that.btnRefresh.linkbutton({
                onClick: function () {
                    that.dtgridEstimate.datagrid('reload');
                }
            });

            // Кнопка Сохранить
            that.btnSave = $('#btnSave', that.element);
            that.btnSave.linkbutton({
                onClick: function () {
                    that._onBtnSaveClick();
                }
            });

            // Кнопка Добавить
            that.btnAdd = $('#btnAdd', that.element);
            that.btnAdd.linkbutton({
                onClick: function () {
                    that._onBtnAddClick();
                }
            });

            // Кнопка Удалить
            that.btnDelete = $('#btnDelete', that.element);
            that.btnDelete.linkbutton({
                onClick: function () {
                    that._onBtnDeleteClick();
                }
            });

            // Кнопка Выгрузить в Excel
            that.btnExcel = $('#btnExcel', that.element);
            that.btnExcel.linkbutton({
                onClick: function () {
                    that._onBtnExcelClick();
                }
            });
        },

        // Кнопка Сохранить
        _onBtnSaveClick: function () {

            var that = this;

            rows = that.dtgridEstimate.datagrid('getData').rows;

            that.dtgridEstimate.datagrid('loading');

            $.ajax({
                url: '/Estimate/Save/',
                type: 'post',
                data: {
                    items: rows
                },
                success: function () {

                    that.dtgridEstimate.datagrid('reload');
                }
            });
        },

        // Кнопка Добавить
        _onBtnAddClick: function () {

            this.dtgridEstimate.datagrid('insertRow', {
                index: 0,
                row: {}
            });

            this.dtgridEstimate.datagrid('editCell', {
                index: 0,
                field: 'specName'
            });

        },

        // Кнопка Удалить
        _onBtnDeleteClick: function () {

            var that = this;

            var rows = this.dtgridEstimate.datagrid('getChecked');

            if (rows.length == 0) {
                $.messager.alert({
                    title: 'Ошибка',
                    icon: 'error',
                    msg: 'Нужно отметить хотя бы одну запись'
                });
                return;
            }

            $.messager.confirm({
                title: 'Подтверждение',
                msg: 'Удалить запись ' + rows.length,
                fn: function (yes) {

                    if (!yes) return;

                    $.each(rows, function (index, row) {

                        var index = that.dtgridEstimate.datagrid('getRowIndex', row);

                        that.dtgridEstimate.datagrid('deleteRow', index);
                    });
                }
            });
        },

        // Кнопка Выгрузить в Excel
        _onBtnExcelClick: function () {

            var that = this;

            that.dtgridEstimate.datagrid('loading', 'Формирование Excel файла...');

            $.ajax({
                url: '/Estimate/CreateExcel',
                type: 'post',
            }).done(function (data) {

                var result = JSON.parse(data);

                that.dtgridEstimate.datagrid('loaded');

                window.location.href = '/Home/GetExcel?key=' + result.key;
            });
        }
    });

})(jQuery);