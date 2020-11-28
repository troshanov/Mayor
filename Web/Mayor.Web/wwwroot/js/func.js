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

    var data = { issueId: issueId };
    $.ajax({
        type: "POST",
        url: "/api/Votes",
        data: JSON.stringify(data),
        success: function (data) {
            $(id).html(data.votesCount);
        },
        contentType: 'application/json',
    });
}

function submitForms(){
  document.getElementById("f0").submit();
  document.getElementById("f1").submit();
  document.getElementById("f2").submit();
}