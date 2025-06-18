import React, { useState } from 'react';
import axios from 'axios';

const App = () => {
    const [newsUrl, setNewsUrl] = useState('');
    const [summary, setSummary] = useState('');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const onSummarize = async () => {
        if (!newsUrl) {
            setError('Please paste an article URL.');
            return;
        }
        setError('');
        setLoading(true);
        setSummary('');
        try {
            const { data } = await axios.post('/api/news/generate', { newsUrl } );
            setSummary(data.summary);
        } catch (err) {
            setError('Something went wrong while generating the summary.');
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="min-vh-100 d-flex align-items-center justify-content-center" style={{
            background: 'linear-gradient(135deg, #1f1c2c, #928dab)',
            padding: '40px'
        }}>
            <div className="container">
                <h1 className="text-center text-white mb-5 display-4 fw-bold">AI Article Summarizer</h1>

                <div className="row justify-content-center g-4">
                    <div className="col-md-7">
                        <div className="bg-white bg-opacity-75 rounded-4 shadow-lg p-4 backdrop-blur">
                            <div className="mb-3">
                                <label className="form-label fw-semibold">Paste article URL</label>
                                <input
                                    type="text"
                                    className="form-control"
                                    placeholder="https://example.com/news/article"
                                    value={newsUrl}
                                    onChange={e => setNewsUrl(e.target.value)}
                                />
                            </div>
                            <div className="d-grid">
                                <button
                                    className="btn btn-dark btn-lg"
                                    onClick={onSummarize}
                                    disabled={loading}
                                >
                                    {loading ? (
                                        <>
                                            <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                                            Generating...
                                        </>
                                    ) : (
                                        'Generate Summary'
                                    )}
                                </button>
                            </div>
                            {error && <div className="alert alert-danger mt-3">{error}</div>}
                        </div>
                    </div>
                </div>

                {summary && (
                    <div className="card mt-5 shadow-lg bg-light border-0 rounded-4">
                        <div className="card-header bg-dark text-white fw-bold rounded-top-4">
                            Generated Summary
                        </div>
                        <div className="card-body">
                            <pre className="card-text text-dark" style={{ whiteSpace: 'pre-wrap' }}>{summary}</pre>
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
};

export default App;