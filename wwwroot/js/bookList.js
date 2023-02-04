//таблица переменыых данных
var dataTable;

// функция загрузки данных
$(document).ready(function () {
    loadDataTable();
});

// загрузка твблицы по индексу, вызов api, запрос на получение, тип данных json, id book
// кнопки удалить и редактирование функция, принимает    индентификатор данных, вызов API внутри контроллера BookController
// многострочный оператор ``, страница редактирования с передачей идентификатора
// ссылка Edit на Upsert
function loadDataTable() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/books/getall",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "width": "20%" },
            { "data": "author", "width": "20%" },
            { "data": "isbn", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                        <a href="/Books/Upsert?id=${data}" class='btn btn-success text-white' style='cursor:pointer; width:70px;'>
                            Edit
                        </a>
                        &nbsp;
                        <a onclick=Delete('/Books/Delete?id='+${data}) class='btn btn-danger text-white' style='cursor:pointer; width:70px;'>
                            Delete
                        </a>
                    </div>`
                }, "width": "40%"
            }
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    })
}

// удаление, необходимо передать id
// сладкое оповещение)) sweet alert
// ответ на ajax
function Delete(url) {
    swal({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover",
        icon: "warning",
        buttons: true,
        //режим для тренировки
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    // перердача сообщения
                    if (data.success) {
                        toastr.success(data.message);
                        // перезагрузка таблицы
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    });
}