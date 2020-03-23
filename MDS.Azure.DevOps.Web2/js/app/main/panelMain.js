(function ($) {

    $.widget("custom.panelMain", {

        _create: function () {

            this._createSettingsPanel();
            this._createFilterPanel();
            this._createActivityGrid();
            this._createMainToolbar();

            this.panelReports = $('#panelReports', this.element);

        },

        _createSettingsPanel: function () {

            var that = this;

            that.panelSettings = $('#panelSettings', that.element);
            that.panelSettings.panelSettings({
                onSave: function () {
                    that.dlistEmployees.datalist('reload');
                    that.panelSettings.window('close');
                },
                onCancel: function () {
                    that.panelSettings.window('close');
                }
            }).window({
                closed: true,
                modal: true,
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
                        field: 'TaskId', title: 'TaskId', sortable: true,
                        formatter: function (val) {
                            return '<div class="fake-link"><a href="#">' + val + '</a></div>'
                        }
                    },
                    { field: 'TaskName', title: 'TaskName', sortable: true },
                    { field: 'ActivityId', title: 'ActivityId', sortable: true },
                    { field: 'ActivityName', title: 'ActivityName', sortable: true },
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
                    { field: 'AssigndTo', title: 'AssigndTo', sortable: true },
                    { field: 'Position', title: 'Position', sortable: true },
                    {
                        field: 'TargetDate', title: 'TargetDate', sortable: true,
                        formatter: function (val) {
                            return $.utils.formatDate(val);
                        }
                    },
                    { field: 'Month', title: 'Month', sortable: true },
                    {
                        field: 'CompletedWork', title: 'CompletedWork', align: 'right', sortable: true,
                        formatter: function (val) {
                            return $.utils.formatHours(val);
                        }
                    },
                    { field: 'ServiceName', title: 'ServiceName', sortable: true },
                ]]
            });

        },

        _createFilterPanel: function () {

            var that = this;

            that.dboxDateFrom = $('#dboxDateFrom', that.element);

            that.dboxDateTo = $('#dboxDateTo', that.element);

            that.dlistEmployees = $('#dlistEmployees', that.element);
            that.dlistEmployees.datalist({
                url: '/Settings/Employees/',
                checkbox: true,
                singleSelect: false,
            });
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
        },

        _onBtnSettingsClick: function () {
            this.panelSettings.panelSettings('load');
            this.panelSettings.window('center');
            this.panelSettings.window('open');
        },

        _onBtnRefreshClick: function () {

            var that = this;

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

            var employees = that.dlistEmployees.datalist('getChecked');

            $.each(employees, function (index, employee) {

                result.employees.push(employee.value);

            });

            return result;
        }

    });

})(jQuery);