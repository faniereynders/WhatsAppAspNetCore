namespace WhatsApp
{
    public static class Scripting
    {
        //Script for send a message by id, example id: 52111111111@c.us
        public static string SendMessageByID = @"
                                                var Chats = Store.Chat.models;
                                                var id = arguments[0];
                                                var message = arguments[1];
                                                for (chat in Chats) {
                                                    if (isNaN(chat)) {
                                                        continue;
                                                    };
                                                    var temp = {};
                                                    temp.contact = Chats[chat].__x__formattedTitle;
                                                    temp.id = Chats[chat].__x_id;
                                                    if(temp.id == id){
                                                        Chats[chat].sendMessage(message);
                                                        return true;
                                                    }
                                                }
                                                return false;
                                           ";



        //Script for send a message by id, example id: 52111111111@c.us
        public static string GetGroups = @"

                                                var GroupOutput = [];

                                                for (group in Groups) {
                                                    if (isNaN(group)) {
                                                        continue;
                                                    }

                                                    //if(!(Groups[group].__x_id.toLowerCase().indexOf('@broadcast') >= 0))

                                                    var group_name = ggn(Groups[group].__x_id);

                                                    var i = Store.GroupMetadata.models[group].participants.models;
                                                    var ii = [];

                                                    for (p in i) {
                                                        if (isNaN(group)) {
                                                            continue;
                                                        };
                                                        var n = Store.GroupMetadata.models[group].participants.models[p].__x_id;
                                                        var m = ggn(Store.GroupMetadata.models[group].participants.models[p].__x_id);
                                                        if (n == null){
                                                            continue;
                                                        }
                                                        ii.push(n);

                                                    }
                                                    GroupOutput.push({
                                                        'Group' : {'Name' :group_name, 'Participants' : ii }
                                                    });

                                                }

                                                function ggn(pno) {
                                                    var contacts = window.Store.Contact.models;
                                                        for(var i in contacts){
                                                        if(isNaN(i)) {
                                                            continue;
                                                        }
                                                        if(pno == contacts[i].__x_id){
                                                            return (contacts[i].__x_name);
                                                        }
                                                    }
                                                }
                                                return GroupOutput;
                                                console.log(GroupOutput);
                                           ";


        public static string SendMessageByNumber(string number, string message)
        {
            var variables = $@"
var contact = ""{number}@c.us"";
var message = ""{message}"";
";

            var sendLogic = variables + @"


var Chats = Store.Chat.models;
var user = Store.Contact.models.find(function (e) { return e.__x_id.search(contact)!=-1 });
Store.Chat.add({ id: user.__x_id, }, { merge: true, add: true, });
for (chat in Chats) {
    if (isNaN(chat)) {
        continue;
    };
    var temp = {};
    temp.contact = Chats[chat].__x_formattedTitle;
    temp.id = Chats[chat].__x_id;
    if(temp.id.search(contact)!=-1 && temp.id.search('g.us')==-1 ){
        Chats[chat].sendMessage(message);
       console.log(""Send this to api as confirmation of sent"");
    }
}
";
            return sendLogic;
        }


        public static string GetUnReadMessages =
            @"function getUnread() {
var Chats = Store.Chat.models;
var Output = [];

function isChatMessage(message) {
    if (message.__x_isSentByMe) {
        return false;
    }
    if (message.__x_isNotification) {
        return false;
    }
    if (!message.__x_isUserCreatedType) {
        return false;
    }
    return true;
}

for (chat in Chats) {
    if (isNaN(chat)) {
        continue;
    };
    var temp = {};
    var contact={};
    contact.Name = Chats[chat].__x_formattedTitle;
    contact.Id = Chats[chat].__x_id;
    temp.messages = [];
    var messages = Chats[chat].msgs.models;

    for (var i = messages.length - 1; i >= 0; i--) {
        if (messages[i].__x_isNewMsg && isChatMessage(messages[i])) {
            temp.messages.push({
                message: messages[i].__x_body,
                timestamp: messages[i].__x_t,
                contact: contact
            });
//set as read
messages[i].__x_isNewMsg=false;
        }
    }
    if (temp.messages.length > 0) {
        Output.push(temp);
    }
}
//console.log(""Unread messages: "", Output);
return Output;
}

return getUnread();
";


    }
}
