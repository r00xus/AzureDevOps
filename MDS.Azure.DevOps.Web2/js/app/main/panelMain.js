(function ($) {

    $.widget("custom.panelMain", {

        _create: function () {

            this._createFilterPanel();
            this._createActivityGrid();
            this._createTaskGrid();
            this._createDiffGrid();
            this._createTimeGrid();
            this._createMainToolbar();

            this.panelReports = $('#panelReports', this.element);

        },

        _createTimeGrid: function () {

            var that = this;

            that.dtgridTime = $('#dtgridTime', that.element);
            that.dtgridTime.datagrid({
                striped: true,
                singleSelect: true,
                rownumbers: true,
                remoteSort: false,
                columns: [[
                    {
                        field: 'employeeName', title: 'Сотрудник', sortable: true,
                    },
                    {
                        field: 'devOpsHours', title: 'Часы по DevOps', sortable: true, align: 'right',
                        formatter: function (val) {
                            return $.utils.formatHours(val);
                        }
                    },
                    {
                        field: 'schedualeHours', title: 'Часы по графику', sortable: true, align: 'right',
                        formatter: function (val) {
                            return $.utils.formatHours(val);
                        }
                    },
                    {
                        field: 'diff', title: 'Разница', sortable: true, align: 'right',
                        formatter: function (val, row, index) {
                            if (val < 0) {
                                return '<span style="color:red">' + $.utils.formatHours(val) + '</span>';
                            }
                            return $.utils.formatHours(val);
                        }
                    },
                ]]
            });

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

        _createTaskGrid: function () {

            var that = this;

            that.dtgridTask = $('#dtgridTask', that.element);
            that.dtgridTask.datagrid({
                striped: true,
                singleSelect: true,
                rownumbers: true,
                remoteSort: false,
                columns: [[
                    {
                        field: 'id', title: 'Task Id', sortable: true,
                        formatter: function (val) {
                            return $.utils.renderIcon('ico-task', 'Задача') + '&nbsp' + $.utils.renderWorkItemLink(val);
                        }
                    },
                    {
                        field: 'taskName', title: 'Task Name', sortable: true, width: 500
                    },
                    {
                        field: 'startDate', title: 'Start Date', sortable: true,
                        formatter: function (val) {
                            return $.utils.formatDate(val);
                        }
                    },
                    {
                        field: 'finishDate', title: 'Finish Date', sortable: true,
                        formatter: function (val) {
                            return $.utils.formatDate(val);
                        }
                    },
                    {
                        field: 'assigndTo', title: 'Assignd To', sortable: true,
                    },
                    {
                        field: 'taskState', title: 'State', sortable: true,
                        formatter: function (val) {
                            return $.utils.renderState(val);
                        }
                    },
                    {
                        field: 'completedWork', title: 'Completed Work', sortable: true, align: 'right',
                        formatter: function (val) {
                            return $.utils.formatHours(val);
                        }
                    },
                    {
                        field: 'originalEstimate', title: 'Original Estimate', sortable: true, align: 'right',
                        formatter: function (val) {
                            return $.utils.formatHours(val);
                        }
                    },
                    { field: 'serviceName', title: 'ServiceName', sortable: true },
                    {
                        field: 'projectOnlineName', title: 'Проект ProjectOnline', sortable: true
                    },
                ]],
                onLoadSuccess: function () {
                    $.utils.setWorkItemLinks($(this).datagrid('getPanel'));
                }
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
                            return $.utils.renderIcon('ico-activity', 'Активность') + '&nbsp' + $.utils.renderWorkItemLink(val);
                        }
                    },
                    { field: 'ActivityName', title: 'ActivityName', sortable: true, width: 500 },
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
                            return $.utils.renderIcon('ico-task', 'Задача') + '&nbsp' + $.utils.renderWorkItemLink(val);
                        }
                    },
                    { field: 'TaskName', title: 'TaskName', sortable: true, width: 500 },
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
                url: ROOT + '/Settings/Employees/',
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

            that.panelReports.panel('loading', 'Выборка данных...');

            $.ajax({
                url: ROOT + '/Home/DevOpsReport/',
                data: {
                    params: params
                },
                datatype: 'json',
                type: 'post',
                success: function (data) {

                    var result = JSON.parse(data);

                    if (!result.success) {
                        $.utils.showError(result);
                        that.panelReports.panel('loaded');
                        return;
                    }

                    that.dtgridActivity.datagrid('loadData', result.activity);
                    that.dtgridDiff.datagrid('loadData', result.diff);
                    that.dtgridTask.datagrid('loadData', result.task);
                    that.dtgridTime.datagrid('loadData', result.time);

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
                url: ROOT + '/Home/CreateExcel',
                type: 'post',
                data: {
                    params: params
                }
            }).done(function (data) {

                var result = JSON.parse(data);

                that.panelReports.panel('loaded');

                window.location.href = ROOT + '/Home/GetExcel?key=' + result.key;
            });
        },

    });

})(jQuery);