(function ($) {

    $.widget("custom.panelSettings", {

        options: {
            onSave: function () { },
            onCancel: function () { }
        },

        _create: function () {

            var that = this;

            that._createEmployeeGrid();
            that._createExcludeGrid();

            that.panelEmployee = $('#panelEmployee', that.element);
            that.panelEmployee.panelEmployee().window({
                closed: true,
                modal: true,
            });

            that._createFooter();
        },

        _createFooter: function () {

            var that = this;

            // Кнопка ОК
            that.btnOk = $('#btnOk', that.element);
            that.btnOk.linkbutton({
                onClick: function () {
                    that._onBtnOkClick();
                }
            });

            // Кнопка Отмена
            that.btnCancel = $('#btnCancel', that.element);
            that.btnCancel.linkbutton({
                onClick: function () {
                    that._onBtnCancelClick();
                }
            });
        },

        _onBtnOkClick: function () {

            this._save();
        },

        _save: function () {

            var that = this;

            that.element.panel('loading', 'Сохранение настроек...');

            $.ajax({
                url: '/Settings/Save',
                type: 'post',
                data: {
                    config: that._getSettings()
                },
                success: function () {

                    that.element.panel('loaded');
                    that.options.onSave();
                }
            });

        },

        _onBtnCancelClick: function () {
            this.options.onCancel();
        },

        _getSettings: function () {

            var settings = {};

            settings.employees = this.dtgridEmployees.datagrid('getData').rows;
            settings.specialDays = this.dtgridExclude.datagrid('getData').rows;

            return settings;
        },

        _setSettings: function (settings) {

            this.dtgridEmployees.datagrid('loadData', settings.employees);

            this.dtgridExclude.datagrid('loadData', settings.specialDays);
        },

        load: function () {

            var that = this;

            that.element.panel('loading', 'Загрузка настроек...');

            $.ajax({
                url: '/Settings/Load/'
            }).done(function (data) {
                that._setSettings(data);
                that.element.panel('loaded');
            });
        },

        _createEmployeeGrid: function () {

            var that = this;

            that.dtgridEmployees = $('#dtgridEmployees', that.element);
            that.dtgridEmployees.datagrid({
                rownumbers: true,
                toolbar: $('#tbarEmployees', that.element),
                checkOnSelect: false,
                selectOnCheck: false,
                ctrlSelect: true,
                dblclickToEdit: true,
                clickToEdit: false,
                striped: true,
                columns: [[
                    {
                        field: 'check', checkbox: true,
                    },
                    {
                        field: 'name', title: 'Сотрудник', width: 300,
                    },
                    {
                        field: 'position', title: 'Должность', width: 300,
                    }
                ]]
            });


            that.btnCreateEmployee = $('#btnCreateEmployee', that.element);
            that.btnCreateEmployee.linkbutton({
                onClick: function () {
                    that._btnCreateEmployeeClick();
                }
            });

            that.btnEditEmployee = $('#btnEditEmployee', that.element);
            that.btnEditEmployee.linkbutton({
                onClick: function () {
                    that._btnEditEmployeeClick();
                }
            });

            that.btnDeleteEmployee = $('#btnDeleteEmployee', that.element);
            that.btnDeleteEmployee.linkbutton({
                onClick: function () {
                    that._btnDeleteEmployee();
                }
            });
        },

        _createExcludeGrid: function () {

            var that = this;

            that.dtgridExclude = $('#dtgridExclude', that.element);
            that.dtgridExclude.datagrid({
                toolbar: $('#tbarExclude', that.element),
                rownumbers: true,
                checkOnSelect: false,
                selectOnCheck: false,
                striped: true,
                singleSelect: true,
                clickToEdit: false,
                dblclickToEdit: true,
                columns: [[
                    {
                        field: 'check', checkbox: true
                    },
                    {
                        field: 'start', title: 'Начало', width: 100,
                        editor: {
                            type: 'datebox'
                        },
                        formatter: function (val) {
                            return $.utils.formatDate(val);
                        }
                    },
                    {
                        field: 'end', title: 'Окончание', width: 100,
                        editor: {
                            type: 'datebox'
                        },
                        formatter: function (val) {
                            return $.utils.formatDate(val);
                        }
                    },
                    {
                        field: 'description', title: 'Описание', width: 220,
                        editor: {
                            type: 'textbox',
                        }
                    },
                    {
                        field: 'hours', title: 'Часы', width: 70, align: 'right',
                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 2,
                            }
                        },
                        formatter: function (val) {
                            return $.utils.formatHours(val);
                        }
                    },
                ]],
            });

            that.dtgridExclude.datagrid('enableCellEditing');

            that.btnCreateExclude = $('#btnCreateExclude', that.element);
            that.btnCreateExclude.linkbutton({
                onClick: function () {
                    that._btnCreateExcludeClick();
                }
            });

            that.btnDeleteExclude = $('#btnDeleteExclude', that.element);
            that.btnDeleteExclude.linkbutton({
                onClick: function () {
                    that._btnDeleteExcludeClick();
                }
            });
        },

        _btnCreateEmployeeClick: function () {

            var that = this;


            this.panelEmployee.panelEmployee('employee', {});
            this.panelEmployee.panelEmployee({
                onSave: function (employee) {

                    that.dtgridEmployees.datagrid('appendRow', employee);

                    that.panelEmployee.window('close');
                },
                onCancel: function () {
                    that.panelEmployee.window('close');
                }
            });
            this.panelEmployee.window('setTitle', 'Добавить');
            this.panelEmployee.window('center');
            this.panelEmployee.window('open');

        },

        _btnEditEmployeeClick: function () {

            var that = this;

            var row = this.dtgridEmployees.datagrid('getSelected');

            if (!row) {
                $.messager.alert({
                    title: 'Ошибка',
                    icon: 'error',
                    msg: 'Выберите сотрудника'
                });
                return;
            }

            this.panelEmployee.panelEmployee('employee', row);
            this.panelEmployee.panelEmployee({
                onSave: function (employee) {

                    var index = that.dtgridEmployees.datagrid('getRowIndex', row);

                    that.dtgridEmployees.datagrid('updateRow', {
                        index: index,
                        row: employee
                    });

                    that.panelEmployee.window('close');
                },
                onCancel: function () {
                    that.panelEmployee.window('close');
                }
            });
            this.panelEmployee.window('setTitle', 'Изменить');
            this.panelEmployee.window('center');
            this.panelEmployee.window('open');
        },

        _btnDeleteEmployee: function () {

            var that = this;

            var rows = this.dtgridEmployees.datagrid('getChecked');

            if (rows.length == 0) {
                $.messager.alert({
                    title: 'Ошибка',
                    icon: 'error',
                    msg: 'Нужно отметить хотя бы одного сотрудника'
                });
                return;
            }

            $.messager.confirm({
                title: 'Подтверждение',
                msg: 'Удалить сотрудников ' + rows.length,
                fn: function (yes) {

                    if (!yes) return;

                    $.each(rows, function (index, row) {

                        var index = that.dtgridEmployees.datagrid('getRowIndex', row);

                        that.dtgridEmployees.datagrid('deleteRow', index);
                    });
                }
            });

        },

        _btnCreateExcludeClick: function () {

            this.dtgridExclude.datagrid('appendRow', {});

            var rows = this.dtgridExclude.datagrid('getData').rows;

            index = rows.length - 1;

            this.dtgridExclude.datagrid('editCell', {
                index: index,
                field: 'start'
            });
        },

        _btnDeleteExcludeClick: function () {

            var that = this;

            var rows = this.dtgridExclude.datagrid('getChecked');

            if (rows.length == 0) {
                $.messager.alert({
                    title: 'Ошибка',
                    icon: 'error',
                    msg: 'Нужно отметить хотя бы одно исключение'
                });
                return;
            }

            $.messager.confirm({
                title: 'Подтверждение',
                msg: 'Удалить исключений ' + rows.length,
                fn: function (yes) {

                    if (!yes) return;

                    $.each(rows, function (index, row) {

                        var index = that.dtgridExclude.datagrid('getRowIndex', row);

                        that.dtgridExclude.datagrid('deleteRow', index);
                    });
                }
            });
        }
    });

})(jQuery);