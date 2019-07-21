import React from 'react';
import { Button, Input, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import axios from 'axios';
import './OfferHeader.css';
import lodash from 'lodash';
import { OfferContext } from '../OfferContext/OfferContext';
import Async from 'react-select/async';

let tmpId = 0;
let newOfferNumber = 0;
let newArticleName = "";
let newArticlePrice = 0;
let offerId = 0;
let tmpArticleId = 0;
let tmpArticle = {};

export default class OfferHeader extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
             id: 0,
             visible: true,
             modalIsOpen: false,
             modalForNewArticleIsOpen: false,
             offerId: null,
             suggestionText: '',
             newOfferNumber: '',
             offerNumber: 0,
             offerCreatedAt: "",
             value: '',
             suggestions: [],
             selectedOption: {}
        }

        this.onTextChange = this.onTextChange.bind(this);
        this.onSelect = this.onSelect.bind(this);
        this.onSuggestionChange = this.onSuggestionChange.bind(this);
        this.toggle = this.toggle.bind(this);
        this.onAddOfferChange = this.onAddOfferChange.bind(this);
        this.toggleNewArticle = this.toggleNewArticle.bind(this);
    }

    removeFromOffer = ({...article}, {...key}) => {
        let newValues = lodash.remove(...this.state.suggestions, value => 
            value.id === article.id
        )

        this.setState(
            testArticleList => {
                testArticleList = newValues
                return testArticleList
            }
        )
    }

    onTextChange = (e) => {
        this.setState({
          offerId: e.target.value
        });
    };

    onAddOfferChange = (e) => {
        newOfferNumber = e.target.value        
    };

    onNewArticleNameChange = (e) => {
        newArticleName = e.target.value        
    };

    onNewArticlePriceChange = (e) => {
        newArticlePrice = e.target.value        
    };

    getData(searchText) {
        let url = "https://localhost:44315/api/Article/Paginated?name="
            + searchText
            + "&skip=0&take=10";

        if(searchText != null) {
            fetch(url)
                .then(response => {
                    return response.data;
                }).catch(error => {
                    throw error;
                });
        }
    }

    onSuggestionChange = (e) => {
        let text = e.target.value;

        this.getData(text);

        this.setState({
          suggestionText: text
        });
    };

    onSelect(val) {
        this.setState({
            value: val
        });
    }

    renderItem(item, isHighlighted){
        return (
            <div style={{ background: isHighlighted ? 'lightgray' : 'white' }}>
                {item.Name}
            </div>   
        ); 
    }

    getItemValue(item){
        return item.Name;
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

    addNewOffer = async (value) => {
        let url = "http://localhost:59189/api/Offer/Create";
        var today = new Date();
        await axios.post(url, {
            data: {
                "Number": newOfferNumber,
                "DateTime": today.getFullYear(),
                "TotalPrice": 0
            }
        }).then(result => {
            value.getOffer(result.data)
        }).catch(error => {
            throw error;
        })
    }

    deleteArticle = async () => {
        let url = "http://localhost:59189/api/Article/Delete?id="
        + tmpArticleId;

        await axios.Delete(url).then(() => {
            alert("Article deleted");
        }).catch(error => {
            alert("Something went wrong");
            throw error;
        });
    }

    updateArticle = async () => {
        let url = "http://localhost:59189/api/Article/Update";

        await axios.Put(url, {
            "Id": tmpArticleId,
            "Name": newArticleName,
            "UnitPrice": newArticlePrice
        }).then(() => {
            alert("Article updated");
        }).catch(error => {
            alert("Something went wrong");
            throw error;
        });
    }

    addNewArticle = async () => {
        let url = "http://localhost:59189/api/Article/Add";

        await axios.post(url, {
            "Name": newArticleName,
            "UnitPrice": newArticlePrice
        }).then(() => {

        }).catch(error => {
            alert("Something went wrong");
            throw error;
        });

        this.setState({
            toggleNewArticle: ! this.state.toggleNewArticle
        });
    }

    deleteArticle = async () => {
        let url = "http://localhost:59189/api/Article/Delete?id="
            + tmpArticleId;

        await axios.delete(url).then(() => {
            alert("Article deleted");
        }).catch(error => {
            alert("Something went wrong");
            throw error;
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
        
                                        <div id="newOffer" className="headerTopElement buttonSpace">
                                            <Button onClick={() => value.getOffer(offerId, {...value})} className="buttonStyle">Search offer</Button>
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
                                            Offer number: {value.offer.Number}
                                        </div>
        
                                        <div id="offerDateTime">
                                            Offer created at: {value.offer.CreatedAt}
                                        </div>
                                    </div>
                                </div>
        
                                <div id="articlesHeader" className="headerBottomStyle">
                                    <div className="articleMiddle">
                                        <div id="selectArticle" className="buttonBottomStyle">
                                            <Async
                                                value={this.state.suggestions}
                                                onChange={e => tmpArticleId = e.value.Id}
                                                loadOptions={this.getData}
                                            />
                                        </div>
        
                                        <div id="addToOffer" className="buttonBottomStyle buttonSpace">
                                            <Button onClick={value.addToCollection}>Add to offer</Button>
                                        </div>
        
                                        <div id="updateArticle" className="buttonBottomStyle buttonSpace">
                                            <Button>Change article</Button>
                                            <Modal isOpen={this.state.modalForNewArticleIsOpen}>
                                                <ModalHeader toggle={this.toggleNewArticle}>New article</ModalHeader>
                                                <ModalBody>
                                                    Enter name for new article:
                                                    <Input value={tmpArticle.Name} onChange={this.onNewArticleNameChange}/>
                                                    <br/>
                                                    Enter price for new article:
                                                    <Input value={tmpArticle.UnitPrice} onChange={this.onNewArticlePriceChange}/>
                                                </ModalBody>
                                                <ModalFooter>
                                                    <Button color="primary" onClick={this.addNewArticle}>Add</Button>
                                                    <Button color="secondary" onClick={this.toggleNewArticle}>Close</Button>
                                                </ModalFooter>
                                            </Modal>
                                        </div>
        
                                        <div id="deleteArticle" className="buttonBottomStyle buttonSpace">
                                            <Button onClick={this.deleteArticle}>Delete article</Button>
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
                                        Object(value.articleCollection).map((article, key) => {
                                            if(article != null) {
                                                return(
                                                    value.renderArticle(article, key)
                                                )
                                            }
                                        })
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
