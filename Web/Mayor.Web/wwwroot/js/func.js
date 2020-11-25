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

function submitForms(){
  document.getElementById("f0").submit();
  document.getElementById("f1").submit();
  document.getElementById("f2").submit();
}