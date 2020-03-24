(function ($) {

    $.widget("custom.panelMain", {

        _create: function () {

            this._createSettingsPanel();
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
                selectOnCheck: false,
                checkOnSelect: false,
                singleSelect: true,
                onLoadSuccess: function () {
                    that._loadSelected();
                }
            });

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

            var employees = that.dlistEmployees.datalist('getChecked');

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

            var rows = that.dlistEmployees.datalist('getData').rows;

            $.each(params.employees, function (index, employee) {

                var finded = $.grep(rows, function (row) { return employee == row.value; });

                if (finded.length == 0) return;

                var rowIndex = that.dlistEmployees.datalist('getRowIndex', finded[0]);

                that.dlistEmployees.datalist('checkRow', rowIndex);

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
        }

    });

})(jQuery);