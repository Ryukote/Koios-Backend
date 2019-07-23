import React from 'react';
import { Button, Input, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import axios from 'axios';
import './OfferHeader.css';
import lodash from 'lodash';
import { OfferContext } from '../OfferContext/OfferContext';
import AsyncSelect from 'react-select/async';

let newArticleName = "";
let newArticlePrice = 0;

export default class OfferHeader extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
             modalIsOpen: false,
             modalForNewArticleIsOpen: false,
             modalForUpdateArticleIsOpen: false,
             offerId: null,
             newOfferNumber: '',
             suggestions: [],
             articleIdToOperate: null,
             selectedArticleName: '',
             selectedArticePrice: null
        }

        this.onTextChange = this.onTextChange.bind(this);
        this.onUpdateArticleNameChange = this.onUpdateArticleNameChange.bind(this);
        this.onUpdateArticlePriceChange = this.onUpdateArticlePriceChange.bind(this);
        this.toggle = this.toggle.bind(this);
        this.onAddOfferChange = this.onAddOfferChange.bind(this);
        this.toggleNewArticle = this.toggleNewArticle.bind(this);
        this.toggleUpdateArticle = this.toggleUpdateArticle.bind(this);
        this.getData = this.getData.bind(this);
        this.articleSelected = this.articleSelected.bind(this);
        this.updateArticle = this.updateArticle.bind(this);
        this.getArticleById = this.getArticleById.bind(this);
    }

    removeFromOffer = ({...article}) => {
        let newValues = lodash.remove(...this.state.suggestions, value => 
            value.id === article.id
        )

        this.setState(
            suggestions => {
                suggestions = newValues
                return suggestions
            }
        )
    }

    onTextChange = (e) => {
        this.setState({
          offerId: e.target.value
        });
    };

    onAddOfferChange = (e) => {
        this.setState({
            newOfferNumber: e.target.value 
        })       
    };

    onNewArticleNameChange = (e) => {
        newArticleName = e.target.value        
    };

    onNewArticlePriceChange = (e) => {
        newArticlePrice = e.target.value        
    };

    onUpdateArticleNameChange = (e) => {
        this.setState({
            selectedArticleName: e.target.value
        })
    }

    onUpdateArticlePriceChange = (e) => {
        this.setState({
            selectedArticePrice: e.target.value
        })
    }

    getArticleById = async () => {
        if(this.state.articleIdToOperate !== null) {
            let url = "https://localhost:44315/api/Article/GetById?id="
            + this.state.articleIdToOperate;

            return await axios.get(url).then(response => {
                let result = response.data;

                this.setState({
                    selectedArticleName: result.name
                })

                this.setState({
                    selectedArticePrice: result.unitPrice
                })

                let articles = {
                    id: this.state.articleIdToOperate,
                    name: this.state.selectedArticleName,
                    unitPrice: this.state.selectedArticePrice
                }

                return articles;
            }).catch(() => alert("You didn't select an article"));
        }
    }

    async getData(searchText) {
        let result;
        let url = "https://localhost:44315/api/Article/Paginated?name="
            + searchText
            + "&skip=0&take=10";

        let dropdownResult = [];

        if(searchText !== '') {
            return await axios.get(url)
                .then(response =>{
                    result = response.data;

                    for(let [key] in Object.entries(result)) {
                        dropdownResult.push({
                            label: result[key].Name,
                            value: result[key].Id
                        })
                    }

                    this.setState({
                        suggestions: dropdownResult
                    });

                    return this.state.suggestions;
                }).catch(error => {
                    throw error;
                });
        }
    }

    toggle() {
        this.setState({
            modalIsOpen: ! this.state.modalIsOpen
        })
    }

    toggleNewArticle() {
        this.setState({
            modalForNewArticleIsOpen: ! this.state.modalForNewArticleIsOpen
        })
    }

    toggleUpdateArticle() {
        this.getArticleById();

        this.setState({
            modalForUpdateArticleIsOpen: ! this.state.modalForUpdateArticleIsOpen
        })
    }

    addNewOffer = async (value) => {
        let url = "http://localhost:59189/api/Offer/Create";
        await axios.post(url, {
                "number": this.state.newOfferNumber,
                "totalPrice": 0
        }).then(result => {
            alert("Offer successfully added");
            this.toggle();
            this.setState({
                offerId: result.data
            })
            value.getOffer(result.data)
        }).catch(error => {
            alert("Offer already exist or you didn't enter valid data");
            this.toggle();
            throw error;
        })
    }

    updateArticle = async () => {
        let url = "http://localhost:59189/api/Article/Update";

        await axios.put(url, {
            "Id": this.state.articleIdToOperate,
            "Name": this.state.selectedArticleName,
            "UnitPrice": this.state.selectedArticePrice
        }).then(() => {
            alert("Article updated");
            this.toggleUpdateArticle();
            // this.setState({
            //     toggleUpdateArticle: false
            // })
        }).catch(error => {
            alert("Something went wrong");
            this.toggleUpdateArticle();
            // this.setState({
            //     toggleUpdateArticle: false
            // })
            throw error;
        });
    }

    addNewArticle = async () => {
        let url = "http://localhost:59189/api/Article/Add";

        await axios.post(url, {
            "Name": newArticleName,
            "UnitPrice": newArticlePrice
        }).then(() => {
            alert("Article successfully added");
        }).catch(error => {
            alert("Something went wrong");
            throw error;
        });

        this.toggleNewArticle();
    }

    deleteArticle = async (data) => {
        let url = "http://localhost:59189/api/Article/Delete?id="
            + this.state.articleIdToOperate;

        await axios.delete(url).then(() => {
            alert("Article successfully deleted");
        }).catch(error => {
            alert("Something went wrong");
            throw error;
        });

        await data.getOffer(data.offerId);
    }

    loadOptions = (inputValue, callback) => {
        setTimeout(() => {
          callback(this.getData(inputValue));
        }, 1000);
    };

    articleSelected = (data) => {
        this.setState({
            articleIdToOperate: data.value
        });
    }

    render() {
        return(
            <OfferContext.Consumer>
                {
                    (value) => {
                        return(
                            <div>
                                <div id="offerHeader">
                                    <div id="headerTop">
                                        <div id="offerSuggestion" className="headerTopElement">
                                            <Input onChange={this.onTextChange} placeholder="enter offer id"/>
                                        </div>
        
                                        <div id="getOffer" className="headerTopElement buttonSpace">
                                            <Button onClick={() => value.getOffer(this.state.offerId)} className="buttonStyle">Search offer</Button>
                                        </div>
        
                                        <div id="newOffer" className="headerTopElement">
                                            <Button className="buttonStyle" onClick={this.toggle.bind(this)}>Create offer</Button>
                                            <Modal isOpen={this.state.modalIsOpen}>
                                                <ModalHeader toggle={this.toggle}>New offer</ModalHeader>
                                                <ModalBody>
                                                    Enter new offer number:
                                                    <Input onChange={this.onAddOfferChange}/>
                                                </ModalBody>
                                                <ModalFooter>
                                                    <Button color="primary" onClick={() => this.addNewOffer({...value})}>Add</Button>
                                                    <Button color="secondary" onClick={this.toggle}>Close</Button>
                                                </ModalFooter>
                                            </Modal>
                                        </div>
                                    </div>
        
                                    <div id="headerBottom">
                                        <div id="offerNumber">
                                            {
                                                value.offer.Number !== 0
                                                ? "Offer number: " + value.offer.Number
                                                : ""
                                            }
                                        </div>
        
                                        <div id="offerDateTime">
                                            {
                                                value.offer.CreatedAt.includes("T")
                                                ? "Offer created at: " + value.offer.CreatedAt
                                                    .split("T")[0]
                                                : ""
                                            }
                                        </div>
                                    </div>
                                </div>
        
                                <div id="articlesHeader" className="headerBottomStyle">
                                    <div className="articleMiddle">
                                        <div id="selectArticle" className="buttonBottomStyle">
                                            <AsyncSelect
                                                options={this.state.suggestions}
                                                loadOptions={this.getData}
                                                onChange={(e) => this.articleSelected(e)}
                                                isClearable={true}
                                            />
                                        </div>
        
                                        <div id="addToOffer" className="buttonBottomStyle buttonSpace">
                                            <Button onClick={async () => {
                                                await this.getArticleById()
                                                    .then(result => {
                                                        value.addToCollection(result, this.state.offerId);
                                                    });
                                                }}>Add to offer</Button>
                                        </div>
        
                                        <div id="updateArticle" className="buttonBottomStyle buttonSpace">
                                            <Button onClick={this.toggleUpdateArticle}>Change article</Button>
                                            <Modal isOpen={this.state.modalForUpdateArticleIsOpen}>
                                                <ModalHeader toggle={this.toggleUpdateArticle}>Update article</ModalHeader>
                                                <ModalBody>
                                                    Change article name:
                                                    <Input value={this.state.selectedArticleName} onChange={this.onUpdateArticleNameChange}/>
                                                    <br/>
                                                    Change article price:
                                                    <Input value={this.state.selectedArticePrice} onChange={this.onUpdateArticlePriceChange}/>
                                                </ModalBody>
                                                <ModalFooter>
                                                    <Button color="primary" onClick={this.updateArticle}>Update</Button>
                                                    <Button color="secondary" onClick={this.toggleUpdateArticle}>Close</Button>
                                                </ModalFooter>
                                            </Modal>
                                        </div>
        
                                        <div id="deleteArticle" className="buttonBottomStyle buttonSpace">
                                            <Button onClick={() => this.deleteArticle(value)}>Delete article</Button>
                                        </div>
        
                                        <div id="createArticle" className="buttonBottomStyle">
                                            <Button onClick={this.toggleNewArticle}>New article</Button>
                                            <Modal isOpen={this.state.modalForNewArticleIsOpen}>
                                                <ModalHeader toggle={this.toggleNewArticle}>New article</ModalHeader>
                                                <ModalBody>
                                                    Enter name for new article:
                                                    <Input onChange={this.onNewArticleNameChange}/>
                                                    <br/>
                                                    Enter price for new article:
                                                    <Input onChange={this.onNewArticlePriceChange}/>
                                                </ModalBody>
                                                <ModalFooter>
                                                    <Button color="primary" onClick={this.addNewArticle}>Add</Button>
                                                    <Button color="secondary" onClick={this.toggleNewArticle}>Close</Button>
                                                </ModalFooter>
                                            </Modal>
                                        </div>
                                    </div>
                                </div>

                                <div id="articleList">
                                    {
                                        value.articleCollection 
                                            ? Object(value.articleCollection).map((article, key) => {
                                                if(article !== null) {
                                                    return(
                                                        value.renderArticle(article, key)
                                                    )
                                                }

                                                return <div></div>
                                            })

                                            : <div></div>
                                    }
                                </div>
                            </div>
                        );
                    }
                }
            </OfferContext.Consumer>
        ); 
    }
}

OfferHeader.contextType = OfferContext;
