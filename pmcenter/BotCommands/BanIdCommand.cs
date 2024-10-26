﻿using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static pmcenter.Methods;

namespace pmcenter.Commands
{
    internal class BanIdCommand : IBotCommand
    {
        public bool OwnerOnly => true;

        public string Prefix => "banid";

        public async Task<bool> ExecuteAsync(TelegramBotClient botClient, Update update)
        {
            try
            {
                BanUser(long.Parse(update.Message.Text.ToLower().Split(" ")[1]));
                _ = await Conf.SaveConf(false, true).ConfigureAwait(false);
                _ = await botClient.SendTextMessageAsync(
                    update.Message.From.Id,
                    Vars.CurrentLang.Message_UserBanned,
                    parseMode: ParseMode.MarkdownV2,
                            protectContent: false,
                            disableNotification: Vars.CurrentConf.DisableNotifications,
                            messageThreadId: update.Message.MessageId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _ = await botClient.SendTextMessageAsync(
                    update.Message.From.Id,
                    Vars.CurrentLang.Message_GeneralFailure.Replace("$1", ex.Message),
                    parseMode: ParseMode.MarkdownV2,
                            protectContent: false,
                            disableNotification: Vars.CurrentConf.DisableNotifications,
                            messageThreadId: update.Message.MessageId).ConfigureAwait(false);
            }
            return true;
        }
    }
}
