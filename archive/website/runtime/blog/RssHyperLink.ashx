<%@ WebHandler Language="C#" Class="RssHyperLinkFromRssFile" %>

using System;
using System.Web;
using System.Collections.Generic;
using RssToolkit.Rss;

public class RssHyperLinkFromRssFile :  RssToolkit.Rss.RssDocumentHttpHandler
{
    protected override void PopulateRss(string channelName, string userName) 
    {
        Rss.Channel = new RssChannel();
        Rss.Version = "2.0";
        Rss.Channel.Title = "RssHyperLink From Rss File";
        Rss.Channel.PubDate = "Tue, 10 Apr 2007 23:01:10 GMT";
        Rss.Channel.LastBuildDate = "Tue, 10 Apr 2007 23:01:10 GMT";
        Rss.Channel.WebMaster = "webmaster@email.com";
        Rss.Channel.Description = "This is to test RssHyperLink From Rss File";
        Rss.Channel.Link = "~/RssHyperLink.aspx";
        
        Rss.Channel.Items = new List<RssItem>();
        if (!string.IsNullOrEmpty(channelName))
        {
            Rss.Channel.Title += " '" + channelName + "'";
        }

        if (!string.IsNullOrEmpty(userName))
        {
            Rss.Channel.Title += " (generated for " + userName + ")";
        }

        RssItem item = new RssItem();
        item.Title = "CodeGeneratedClass";
        item.Description = "Consuming RSS feed programmatically using strongly typed classes";
        item.Link = "~/CodeGeneratedClass.aspx";
        Rss.Channel.Items.Add(item);

        item = new RssItem();
        item.Title = "ObjectDataSource";
        item.Description = "Consuming RSS feed using ObjectDataSource";
        item.Link = "~/ObjectDataSource.aspx";
        Rss.Channel.Items.Add(item);

        item = new RssItem();
        item.Title = "Opml";
        item.Description = "Using OPML Files to aggregate and produce one feed";
        item.Link = "~/Opml.aspx";
        Rss.Channel.Items.Add(item);

        item = new RssItem();
        item.Title = "RssDataSource";
        item.Description = "Consuming RSS feed using RssDataSource";
        item.Link = "~/RssDataSource.aspx";
        Rss.Channel.Items.Add(item);
        
        item = new RssItem();
        item.Title = "RssDocument";
        item.Description = "Consuming RSS feed programmatically using RssDocument";
        item.Link = "~/RssDocument.aspx";
        Rss.Channel.Items.Add(item);
    }    
}