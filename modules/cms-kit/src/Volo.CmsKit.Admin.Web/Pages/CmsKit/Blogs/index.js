
(function ($) {
    var l = abp.localization.getResource("CmsKit");

    var createModal = new abp.ModalManager({ viewUrl: abp.appPath + "CmsKit/Blogs/CreateModal", modalClass: 'createBlog' });
    var updateModal = new abp.ModalManager({ viewUrl: abp.appPath + "CmsKit/Blogs/UpdateModal", modalClass: 'updateBlog' });
    var featuresModal = new abp.ModalManager(abp.appPath + "CmsKit/Blogs/FeaturesModal");

    var blogsService = volo.cmsKit.admin.blogs.blogAdmin;

    abp.ui.extensions.entityActions.get('cmskit.blog').addContributor(
        function (actionList) {
            return actionList.addManyTail(
                [
                    {
                        text: l('Features'),
                        visible: abp.auth.isGranted('CmsKit.Blogs.Features'),
                        action: function (data) {
                            featuresModal.open({ blogId: data.record.id });
                        }
                    },
                    {
                        text: l('Edit'),
                        visible: abp.auth.isGranted('CmsKit.Blogs.Update'),
                        action: function (data) {
                            updateModal.open({ id: data.record.id });
                        }
                    },
                    {
                        text: l('Delete'),
                        visible: abp.auth.isGranted('CmsKit.Blogs.Delete'),
                        confirmMessage: function (data) {
                            return l("BlogDeletionConfirmationMessage", data.record.name)
                        },
                        action: function (data) {
                            blogsService
                                .delete(data.record.id)
                                .then(function () {
                                    abp.notify.info(l("SuccessfullyDeleted"));
                                    dataTable.ajax.reload();
                                });
                        }
                    }
                ]
            );
        }
    );

    abp.ui.extensions.tableColumns.get('cmskit.blog').addContributor(
        function (columnList) {
            columnList.addManyTail(
                [
                    {
                        title: l("Details"),
                        targets: 0,
                        rowAction: {
                            items: abp.ui.extensions.entityActions.get('cmskit.blog').actions.toArray()
                        }
                    },
                    {
                        title: l("Name"),
                        orderable: true,
                        data: "name"
                    },
                    {
                        title: l("Slug"),
                        orderable: true,
                        data: "slug"
                    }
                ]
            );
        },
        0 //adds as the first contributor
    );

    $(function () {
        var _$wrapper = $('#CmsKitBlogsWrapper');
        var _$table = _$wrapper.find('table');

        var dataTable = _$table.DataTable(
            abp.libs.datatables.normalizeConfiguration({
                processing: true,
                serverSide: true,
                paging: true,
                searching: false,
                autoWidth: false,
                scrollCollapse: true,
                scrollX: true,
                ordering: true,
                order: [[1, "desc"]],
                ajax: abp.libs.datatables.createAjax(blogsService.getList),
                columnDefs: abp.ui.extensions.tableColumns.get('cmskit.blog').columns.toArray()
            })
        );

        $('#AbpContentToolbar button[name=CreateBlog]').on('click', function (e) {
            e.preventDefault();
            createModal.open();
        });

        createModal.onResult(function () {
            dataTable.ajax.reload();
        });

        updateModal.onResult(function () {
            dataTable.ajax.reload();
        });
    }); 
})(jQuery);