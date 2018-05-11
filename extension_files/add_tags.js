async function addFlairs() { 
console.log("dota2tagger Running!"); 
let titles = document.getElementsByClassName("title"); // Get all elements that could have tags
console.log(`Length: ${titles.length}`);
for (let element of titles) {
    if (element.tagName != "P") {  // the "P" elements contains the titles and tags for posts
        continue;
    }
    let inner_html = element.innerHTML;
    if (!inner_html.includes("linkflairlabel")) { // Add a link flair if the post doesn't already have one

        let title = inner_html.slice(0, inner_html.indexOf("</a>"));
        title = title.slice(title.lastIndexOf(">") + 1);    // Extract the title
        console.log(title);
        let url = inner_html.slice(0, inner_html.lastIndexOf("</a>"));
        url = url.slice(url.lastIndexOf(">") + 1);          // Extract url (domain)
        console.log(url);

        let tag = await predictTag(title, url, "");  // I don't currently have a method of passing the body
        let flair = `Predicted | ${tag}`;
        let html_flair = `<span class="linkflairlabel" title="${flair}">${flair}</span>`;

        let start_index = inner_html.indexOf("</a>") + 4;   // Find where to put the flair
        new_html = inner_html.slice(0, start_index) + html_flair + inner_html.slice(start_index);  // Put in flair
        element.innerHTML = new_html;  // Replace HTML with HTML including flair
    }
}
}


// https://docs.microsoft.com/en-us/azure/machine-learning/studio/consume-web-services
async function predictTag(title, url, body) {
    // let req = require("request");

    const wrapper_uri = "https://dota2tagger.azurewebsites.net/api/gettag?code=zAARE6Okj1AGYrzyAEtytRHTbvdp0MwVkE0bU8wp7ltDvZJ/tfrEtA==";
    const apiKey = "YaQH0mqSuGumNcI4YYMmZirFFxqT4R+lRZRFQ6MeMmM7lrN9FRhGbUmbGPb7QJrBMJ1iYB4gcRDnH/VhnS8iRQ==";
    //URI and ApiKey for an azure function that then calls Azure Machine Learning Services (roundabout method required do to CORS restrictions)
    let data = {
        "Inputs": {
          "input1": [
            {
              "Col1": title,
              "Col2": url,
              "Col3": body,
              "Col4": ""
            }
          ]
        },
        "GlobalParameters": {}
    }

    const options = {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + apiKey
        },
        body: JSON.stringify(data),
    }

    try {
        let response = await fetch(wrapper_uri, options);
        if (response.ok) {
            let jsonResponse = await response.json();
            let tag = jsonResponse.slice(41, -5) // Extracts the actual tag
            return tag;
        } else {
            console.log("not ok! : " + response); // If something went wrong
        }
    } catch (error) {
        console.log(error);  // If error is thrown
    }
}


addFlairs();