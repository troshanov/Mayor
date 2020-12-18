function func(a)
{
    if(a==1){
        document.getElementById("f2").style.display="none";
        document.getElementById("f1").style.display="block";
    }
    else{
        document.getElementById("f1").style.display="none";
        document.getElementById("f2").style.display="block";
    }
}

function vote(issueId, voteElement){
    
    var id = '#' + voteElement.id;
    console.log(issueId, id);
    var antiForgeryToken = $('#antiForgeryForm input[name=__RequestVerificationToken]').val();
    var data = { issueId: issueId };
    $.ajax({
        type: "POST",
        url: "/api/Votes",
        headers: {
            'X-CSRF-TOKEN': antiForgeryToken
        },
        data: JSON.stringify(data),
        success: function (data) {
            $(id).html(data.votesCount);
        },
        contentType: 'application/json',
    });
}

// function commentsNext(page, issueId){
// var id = '#comment-section';
// var data = {IssueId: issueId, Page: page};
// $.ajax({
//     type: 'POST',
//     url: '/Comments/GetComments',
//     data: JSON.stringify(data),
//     success: function (data) { 
//         $(id).html(data);
//         $('#prev-page').attr('onclick','commentsPrev(' + (page - 1) +', ' + issueId +')');
//         $('#next-page').attr('onclick','commentsNext(' + (page + 1) +', ' + issueId +')');
//     },
//     contentType: 'application/json',
// });
// }


// function commentsPrev(page, issueId){
//     var id = '#comment-section';
//     var data = {IssueId: issueId, Page: page};
//     $.ajax({
//         type: 'POST',
//         url: '/Comments/GetComments',
//         data: JSON.stringify(data),
//         success: function (data) {
//             $(id).html(data);
//             $('#prev-page').attr('onclick','commentsPrev(' + (page + 1) +', ' + issueId +')');
//             $('#next-page').attr('onclick','commentsNext(' + (page - 1) +', ' + issueId +')');
//         },
//         contentType: 'application/json',
//     });
// }

function submitForms(){
  document.getElementById("f0").submit();
  document.getElementById("f1").submit();
  document.getElementById("f2").submit();
}