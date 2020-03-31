(function ($) {

    $.widget("custom.panelMain", {

        _create: function () {

            this._createFilterPanel();
            this._createActivityGrid();
            this._createDiffGrid();
            this._createMainToolbar();

            this.panelReports = $('#panelReports', this.element);

        },

        _createDiffGrid: function () {

            var that = this;

            that.dtgridDiff = $('#dtgridDiff', that.element);
            that.dtgridDiff.datagrid({
                striped: true,
                singleSelect: true,
                rownumbers: true,
                remoteSort: false,
                columns: [[
                    {
                        field: 'employeeName', title: 'Сотрудник', sortable: true,
                    },
                    {
                        field: 'day', title: 'Дата', sortable: true,
                        formatter: function (val) {
                            return $.utils.formatDate(val);
                        }
                    },
                    {
                        field: 'dayOfWeek', title: 'День недели', sortable: true
                    },
                    {
                        field: 'description', title: 'Тип дня', sortable: true
                    },
                    {
                        field: 'devOpsHours', title: 'Часы по DevOps', align: 'right', sortable: true,
                        formatter: function (val) {
                            return $.utils.formatHours(val);
                        }
                    },
                    {
                        field: 'schedualeHours', title: 'Часы по графику', align: 'right', sortable: true,
                        formatter: function (val) {
                            return $.utils.formatHours(val);
                        }
                    },
                    {
                        field: 'diff', title: 'Разница', align: 'right', sortable: true,
                        formatter: function (val) {
                            return $.utils.formatHours(val);
                        }
                    }
                ]]
            });

        },

        _createActivityGrid: function () {

            var that = this;

            that.dtgridActivity = $('#dtgridActivity', that.element);
            that.dtgridActivity.datagrid({
                striped: true,
                singleSelect: true,
                rownumbers: true,
                remoteSort: false,
                //nowrap: false,
                columns: [[
                    {
                        field: 'ActivityId', title: 'ActivityId', sortable: true,
                        formatter: function (val) {
                            return $.utils.renderWorkItemLink(val);
                        }
                    },
                    { field: 'ActivityName', title: 'ActivityName', sortable: true },
                    {
                        field: 'TargetDate', title: 'TargetDate', sortable: true,
                        formatter: function (val) {
                            return $.utils.formatDate(val);
                        }
                    },
                    { field: 'AssigndTo', title: 'AssigndTo', sortable: true },
                    {
                        field: 'TaskId', title: 'TaskId', sortable: true,
                        formatter: function (val) {
                            return $.utils.renderWorkItemLink(val);
                        }
                    },
                    { field: 'TaskName', title: 'TaskName', sortable: true },
                    {
                        field: 'StartDate', title: 'StartDate', sortable: true,
                        formatter: function (val) {
                            return $.utils.formatDate(val);
                        }
                    },
                    {
                        field: 'FinishDate', title: 'FinishDate', sortable: true,
                        formatter: function (val) {
                            return $.utils.formatDate(val);
                        }
                    },
                    { field: 'Position', title: 'Position', sortable: true },
                    { field: 'Month', title: 'Month', sortable: true },
                    {
                        field: 'CompletedWork', title: 'CompletedWork', align: 'right', sortable: true,
                        formatter: function (val) {
                            return $.utils.formatHours(val);
                        }
                    },
                    { field: 'ServiceName', title: 'ServiceName', sortable: true },
                ]],
                onLoadSuccess: function () {
                    $.utils.setWorkItemLinks($(this).datagrid('getPanel'));
                }
            });
        },

        _createFilterPanel: function () {

            var that = this;

            that.dboxDateFrom = $('#dboxDateFrom', that.element);

            that.dboxDateTo = $('#dboxDateTo', that.element);

            that.dtgridEmployees = $('#dtgridEmployees', that.element);
            that.dtgridEmployees.datagrid({
                url: '/Settings/Employees/',
                checkbox: true,
                singleSelect: false,
                selectOnCheck: false,
                checkOnSelect: false,
                singleSelect: true,
                fitColumns: true,
                remoteSort: false,
                columns: [[
                    { field: 'check', checkbox: true },
                    { field: 'text', title: 'Сотрудники', width: 100, sortable: true }
                ]],
                onLoadSuccess: function () {
                    that._loadSelected();
                }
            }).datagrid('getPanel').addClass('lines-no');

            that._loadFilter();
        },

        _createMainToolbar: function () {

            var that = this;

            that.btnSettings = $('#btnSettings', that.element);
            that.btnSettings.linkbutton({
                onClick: function () {
                    that._onBtnSettingsClick();
                }
            });

            that.btnRefresh = $('#btnRefresh', that.element);
            that.btnRefresh.linkbutton({
                onClick: function () {
                    that._onBtnRefreshClick();
                }
            });

            that.btnExcel = $('#btnExcel', that.element);
            that.btnExcel.linkbutton({
                onClick: function () {
                    that._onBtnExcelClick();
                }
            });
        },

        _onBtnSettingsClick: function () {
            this.panelSettings.panelSettings('load');
            this.panelSettings.window('center');
            this.panelSettings.window('open');
        },

        _onBtnRefreshClick: function () {

            var that = this;

            that._saveFilter();

            var params = that._getParams();

            that.panelReports.panel('loading', 'Выборка данных');

            $.ajax({
                url: '/Home/DevOpsReport/',
                data: {
                    params: params
                },
                datatype: 'json',
                type: 'post',
                success: function (data) {

                    var result = JSON.parse(data);

                    that.dtgridActivity.datagrid('loadData', result.activity);
                    that.dtgridDiff.datagrid('loadData', result.diff);

                    that.panelReports.panel('loaded');

                }
            });
        },

        _getParams: function () {

            var that = this;

            var result = {
                employees: [],
                start: that.dboxDateFrom.datebox('getValue'),
                end: that.dboxDateTo.datebox('getValue')
            };

            var employees = that.dtgridEmployees.datagrid('getChecked');

            $.each(employees, function (index, employee) {

                result.employees.push(employee.value);

            });

            return result;
        },

        _setParams: function (params) {

            var that = this;

            that.dboxDateFrom.datebox('setValue', params.start);

            that.dboxDateTo.datebox('setValue', params.end);

        },

        _loadSelected: function () {

            var that = this;

            if (!$.cookie('params')) return;

            params = JSON.parse($.cookie('params'));

            var rows = that.dtgridEmployees.datagrid('getData').rows;

            $.each(params.employees, function (index, employee) {

                var finded = $.grep(rows, function (row) { return employee == row.value; });

                if (finded.length == 0) return;

                var rowIndex = that.dtgridEmployees.datagrid('getRowIndex', finded[0]);

                that.dtgridEmployees.datagrid('checkRow', rowIndex);

            });

        },

        _saveFilter: function () {

            var params = this._getParams();

            $.cookie('params', JSON.stringify(params));
        },

        _loadFilter: function () {

            var params = $.cookie('params');

            if (!params) return;

            params = JSON.parse($.cookie('params'));

            this._setParams(params);
        },

        _onBtnExcelClick: function () {

            var that = this;

            that.panelReports.panel('loading', 'Формирование Excel файла...');

            var params = that._getParams();

            $.ajax({
                url: '/Home/CreateExcel',
                type: 'post',
                data: {
                    params: params
                }
            }).done(function (data) {

                var result = JSON.parse(data);

                that.panelReports.panel('loaded');

                window.location.href = '/Home/GetExcel?key=' + result.key;
            });
        },

    });

})(jQuery);