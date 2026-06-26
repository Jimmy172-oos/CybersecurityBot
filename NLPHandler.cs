using System;

namespace CybersecurityBot
{
    class NLPHandler
    {
        public string DetectIntent(string input)
        {
            string text = input.ToLower();

            if (text.Contains("help") || text.Contains("menu") || text.Contains("what can you do"))
                return "help";

            if (text.Contains("quiz") || text.Contains("game") || text.Contains("play") || text.Contains("test me"))
                return "quiz";

            if (text.Contains("log") || text.Contains("history") || text.Contains("what have you done"))
                return "log";

            if (text.Contains("add") || text.Contains("new") || text.Contains("create") || text.Contains("remind"))
                return "task";

            if (text.Contains("show") || text.Contains("view") || text.Contains("list") || text.Contains("my tasks"))
                return "view";

            if (text.Contains("complete") || text.Contains("done") || text.Contains("finish"))
                return "complete";

            if (text.Contains("delete") || text.Contains("remove"))
                return "delete";

            return "chat";
        }
    }
}