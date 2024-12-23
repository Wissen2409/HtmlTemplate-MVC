using Microsoft.AspNetCore.Mvc.ViewFeatures;

public static class BreadCrumbViewBagHelper
{
    /// <summary>
    /// ViewData'ya sayfadaki breadcrumb verisini ekler. Sayfanin path'ine gore istenildigi kadar breadcrumb eklenebilir.
    /// Ornek kullanim icin Shop/Index actionuna bak!!!
    /// </summary>
    /// <param name="viewData"> Controller icindeki ViewData nesnesini buraya gonderin</param>
    /// <param name="items">Breadcrumb ogeleri (isim ve URL cifti olarak)!! Son ogenin URL'si null birakilmali !! </param>
    public static void SetBreadCrumb(ViewDataDictionary viewData, params (string Name, string Url)[] items)
    {
        // parametre olarak gelen name ve url alanlarini breadcrumbitem tipinde bir nesne icine yerlestirip liste haline getir
        var breadcrumbItems = items.Select(item => new BreadcrumbItem
        {
            Name = item.Name,
            Url = item.Url
        }).ToList();

        viewData["Breadcrumb"] = breadcrumbItems; // parametre olarak verilen viewData icindeki 'Breadcrumb' keyinin oldugu yere bu listeyi koy.
    }
}

public class BreadcrumbItem
{
    public string Name { get; set; }
    public string Url { get; set; }
}