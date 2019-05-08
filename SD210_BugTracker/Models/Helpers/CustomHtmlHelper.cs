using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SD210_BugTracker.Models.Helpers
{
    public static class CustomHtmlHelper
    {
        public static MvcHtmlString EditComment(this HtmlHelper html, int tId, int cId, string comment, string urlAction)
        {
           
            return MvcHtmlString.Create(
            $@"<!-- Button trigger modal -->
            <button  class='btn-xs btn-info' data-toggle='modal' data-target='#E{cId}'>
              Edit
            </button>&nbsp

            <!-- Modal -->
            <div class='modal fade' id='E{cId}' tabindex='-1' role='dialog' aria-labelledby='exampleModalLabel' aria-hidden='true'>
                <div class='modal-dialog' role='document'>
                    <div class='modal-content'>
                        <div class='modal-header'>
                            <h5 class='modal-title' id='exampleModalLabel'>Edit Comment</h5>
                            <button type='button' class='close' data-dismiss='modal' aria-label='Close'>
                                <span aria-hidden='true'>&times;</span>
                            </button>
                        </div>
                        <div  class='modal-body'>
                            <form action='{urlAction}' , class='form - inline' method='post' >                               
                                <input type = 'hidden' name = 'TId' value ={tId} />
                                <input type = 'hidden' name = 'CId' value ={cId} />
                                <input type = 'text' name = 'Comment' class='form-control' value='{comment}'>   

                                  <div class='modal-footer'>
                                    <button type='button' class='btn btn-secondary' data-dismiss='modal'>Close</button>
                                    <button type='submit' class='btn btn-primary'>Save</button>                          
                                 </div>
                            </form>
                        </div>                        
                    </div>
                </div>
            </div>");
        }

        public static MvcHtmlString DeleteComment(this HtmlHelper html,int tId, int cId, string comment, string urlAction)
        {
            return MvcHtmlString.Create(
            $@"<!-- Button trigger modal -->
            <button  class='btn-xs btn-danger' data-toggle='modal' data-target='#D{cId}'>
              Delete
            </button>

            <!-- Modal -->
            <div class='modal fade' id='D{cId}' tabindex='-1' role='dialog' aria-labelledby='exampleModalLabel' aria-hidden='true'>
                <div class='modal-dialog' role='document'>
                    <div class='modal-content'>
                        <div class='modal-header'>
                            <h5 class='modal-title' id='exampleModalLabel'>Delete Comment</h5>
                            <button type='button' class='close' data-dismiss='modal' aria-label='Close'>
                                <span aria-hidden='true'>&times;</span>
                            </button>
                        </div>
                        <div  class='modal-body'>
                            <form action='{urlAction}' , class='form - inline' method='post' >  
                                <input type = 'hidden' name = 'TId' value ={tId} />
                                <input type = 'hidden' name = 'CId' value ={cId} />
                                 <div class='text-left'>{comment}</div></br>   
                                 Are you sure to delete this comment?
                                  <div class='modal-footer'>
                                    <button type='button' class='btn btn-secondary' data-dismiss='modal'>Close</button>
                                    <button type='submit' class='btn btn-danger'>Delete</button>                          
                                 </div>
                            </form>
                        </div>                        
                    </div>
                </div>
            </div>");
        }

        public static MvcHtmlString DeleteFile(this HtmlHelper html, int tId, int fId, string fileName, string urlAction)
        {
            return MvcHtmlString.Create(
            $@"<!-- Button trigger modal -->
            <button  class='btn-xs btn-danger' data-toggle='modal' data-target='#A{fId}'>
              Delete
            </button>

            <!-- Modal -->
            <div class='modal fade' id='A{fId}' tabindex='-1' role='dialog' aria-labelledby='exampleModalLabel' aria-hidden='true'>
                <div class='modal-dialog' role='document'>
                    <div class='modal-content'>
                        <div class='modal-header'>
                            <h5 class='modal-title' id='exampleModalLabel'>Delete File</h5>
                            <button type='button' class='close' data-dismiss='modal' aria-label='Close'>
                                <span aria-hidden='true'>&times;</span>
                            </button>
                        </div>
                        <div  class='modal-body'>
                            <form action='{urlAction}' , class='form - inline' method='post' >  
                                <input type = 'hidden' name = 'TicketId' value ={tId} />
                                <input type = 'hidden' name = 'Id' value ={fId} />
                                <input type = 'hidden' name = 'FilePath' value ={fileName} />
                                 <div class='text-left'>{fileName}</div></br>   
                                 Are you sure to delete this file?
                                  <div class='modal-footer'>
                                    <button type='button' class='btn btn-secondary' data-dismiss='modal'>Close</button>
                                    <button type='submit' class='btn btn-danger'>Delete</button>                          
                                 </div>
                            </form>
                        </div>                        
                    </div>
                </div>
            </div>");
        }
    }
}