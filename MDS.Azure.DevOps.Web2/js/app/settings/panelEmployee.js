(function ($) {

    $.widget("custom.panelEmployee", {

        options: {
            onSave: function (employee) { },
            onCancel: function () { }
        },

        _create: function () {

            this.layout = $('#layout', this.element);

            this._createForm();
            this._createWeekGrid();
            this._createExcludeGrid();
            this._createFooter();

            this.tabSchedual = $('#tabSchedual', this.element);

            this.layout.layout('resize');
        },

        _createForm: function () {

            this.tboxName = $('#tboxName', this.element);
            this.tboxName.textbox({
                label: 'Сотрудник'
            });

            this.cboxPosition = $('#cboxPosition', this.element);
            this.cboxPosition.combobox({
                label: 'Должность',
                panelHeight: 'auto',
                data: [
                    { value: 'менеджер', text: 'менеджер' },
                    { value: 'ведущий инженер-программист', text: 'ведущий инженер-программист' },
                    { value: 'старший инженер-программист', text: 'старший инженер-программист' },
                    { value: 'инженер-программист', text: 'инженер-программист' },
                ]
            });
        },

        _createWeekGrid: function () {

            var that = this;

            that.dtgridWeek = $('#dtgridWeek', that.element);
            that.dtgridWeek.datagrid({
                rownumbers: true,
                singleSelect: true,
                striped: true,
                columns: [[
                    {
                        field: 'dayOfWeek', title: 'День недели', width: 200
                    },
                    {
                        field: 'hours', title: 'Часы', width: 100, align: 'right',
                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 2,
                                decimalSeparator: ','
                            }
                        },
                        formatter: function (val) {
                            return $.utils.formatHours(val);
                        }
                    },
                ]],
            });

            that.dtgridWeek.datagrid('enableCellEditing');
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

        _createFooter: function () {

            var that = this;

            // Кнопка ОК
            that.btnOk = $('#btnOk', that.element);
            that.btnOk.linkbutton({
                onClick: function () {
                    that._btnOkClick();
                }
            });

            // Кнопка Отмена
            that.btnCancel = $('#btnCancel', that.element);
            that.btnCancel.linkbutton({
                onClick: function () {
                    that._btnCancelClick();
                }
            });
        },

        // Кнопка ОК
        _btnOkClick: function () {
            var employee = this._getEmployee();
            this.options.onSave(employee);
        },

        // Кнопка Отмена
        _btnCancelClick: function () {
            this.options.onCancel();
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
                    msg: 'Нужно отметить хотя бы одну исключение'
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
        },

        employee: function (val) {

            if (val === undefined) return this._getEmployee();

            this._setEmployee(val);

        },

        _getEmployee: function () {

            var that = this;

            var employee = {};

            employee.name = that.tboxName.textbox('getValue');

            employee.position = that.cboxPosition.textbox('getValue');

            employee.specialDays = that.dtgridExclude.datagrid('getData').rows;

            employee.customDays = [];

            var daysOfWeek = that.dtgridWeek.datagrid('getData').rows;

            $.each(daysOfWeek, function (index, dayOfWeek) {

                if (dayOfWeek.hours != '')
                    employee.customDays.push({
                        dayOfWeek: index + 1,
                        hours: dayOfWeek.hours
                    });
            });

            return employee;
        },

        _setEmployee: function (employee) {

            var that = this;

            that.clear();

            if (employee.name !== undefined)
                that.tboxName.textbox('setValue', employee.name);

            if (employee.position !== undefined)
                that.cboxPosition.textbox('setValue', employee.position);

            if (employee.specialDays !== undefined)
                that.dtgridExclude.datagrid('loadData', employee.specialDays);


            if (employee.customDays !== undefined) {

                var getDayName = function (day) {
                    switch (day) {
                        case 1: return 'Понедельник';
                        case 2: return 'Вторник';
                        case 3: return 'Среда';
                        case 4: return 'Четверг';
                        case 5: return 'Пятница';
                        case 6: return 'Суббота';
                        case 7: return 'Воскресенье';
                        default: return '';
                    }
                };

                for (var day = 1; day <= 7; day++) {

                    var dayOfWeek = {
                        dayOfWeek: getDayName(day),
                        hours: null
                    };

                    var finded = $.grep(employee.customDays, function (customDay) {
                        return customDay.dayOfWeek == day;
                    });

                    if (finded.length != 0) {
                        dayOfWeek.hours = finded[0].hours
                    }

                    that.dtgridWeek.datagrid('appendRow', dayOfWeek);
                }
            }

            that.tabSchedual.tabs('select', 0);
        },

        clear: function () {

            var that = this;

            that.tboxName.textbox('clear');
            that.cboxPosition.textbox('clear');
            that.dtgridExclude.datagrid('loadData', []);
            that.dtgridWeek.datagrid('loadData', []);
        }

    });

})(jQuery);