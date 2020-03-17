(function ($) {

    $.widget("custom.panelMain", {

        _create: function () {

            this._createSettingsPanel();
            this._createFilterPanel();
            this._createActivityGrid();
            this._createMainToolbar();

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
                columns: [[
                    { field: 'taskId', title: 'TaskId' },
                    { field: 'taskName', title: 'TaskName' },
                    { field: 'activityId', title: 'ActivityId' },
                    { field: 'activityName', title: 'ActivityName' },
                    { field: 'startDate', title: 'StartDate' },
                    { field: 'finishDate', title: 'FinishDate' },
                    { field: 'assigndTo', title: 'AssigndTo' },
                    { field: 'position', title: 'Position' },
                    { field: 'targetDate', title: 'TargetDate' },
                    { field: 'month', title: 'Month' },
                    { field: 'completedWork', title: 'CompletedWork' },
                    { field: 'serviceName', title: 'ServiceName' },
                ]]
            });

        },

        _createFilterPanel: function () {

            var that = this;

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
        },

        _onBtnSettingsClick: function () {
            this.panelSettings.panelSettings('load');
            this.panelSettings.window('center');
            this.panelSettings.window('open');
        }

    });

})(jQuery);