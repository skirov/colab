define(['knockout', 'text!./all-issues.html', 'issueProvider'], function (ko, templateMarkup, IssuesProvider) {

    function AllIssues(params) {
        var that = this;
        that.isInitialized = ko.observable();
        that.allIssues = ko.observable();

        IssuesProvider.getAll(params.id)
            .done(function (data) {
                that.allIssues(data);
                that.isInitialized(true);
            });
    }

    // This runs when the component is torn down. Put here any logic necessary to clean up,
    // for example cancelling setTimeouts or disposing Knockout subscriptions/computeds.
    AllIssues.prototype.dispose = function () {
    };

    return {viewModel: AllIssues, template: templateMarkup};

});
