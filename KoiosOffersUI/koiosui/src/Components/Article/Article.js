import axios from 'axios';

export const saveArticle = async (article) => {
    await axios.post('http://localhost:59189/api/Article/Add', {
        "name": article.name,
        "unitPrice": article.unitPrice
    }).then(response => {
        return response.data;
    }).catch(error => {
        alert("Something went wrong");
        throw error;
    });
}

export const getSuggestionsByName = async (name, skip, take) => {
    let result;
    await axios.get('http://localhost:59189/api/Article/Paginated?' +
     'name=' + name + 
     '&skip=' + skip +
     '&take=' + take)
        .then(response => {
            result = response.data;
        }).catch(error => {
            result = error;
        });

    return result;
};

export const deleteArticle = async (id) => {
    await axios.delete('http://localhost:59189/api/Article/Delete?id=' + id)
        .then(response => {
            return {status: response.status};
        }).catch(() => {
            alert("Something went wrong");
            return {status: 400};
        });
}

export const updateArticle = async (article) => {
    await axios.put('http://localhost:59189/api/Article/Update', {
        "id": article.id,
        "name": article.name,
        "unitPrice": article.unitPrice
    }).then(response => {
        return {
            status: response.status
        }
    }).catch(() => {
        alert("Something went wrong");
        return {
            status: 400
        }
    });
}

export const getAllArticles = async (article) => {
    let result;
    await axios.put('http://localhost:59189/api/Article/Update', {
        "id": article.id,
        "name": article.name,
        "unitPrice": article.unitPrice
    }).then(response => {
        return {
            status: response.status
        }
    }).catch(() => {
        return {
            status: 400
        }
    });
}

export default Article;